using System.Security.Claims;
using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Common.Behaviors;

public sealed class UserSyncBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly string[] EmailClaimTypes = [ClaimTypes.Email, "email"];
    private static readonly string[] UserIdClaimTypes = [ClaimTypes.NameIdentifier, "sub"];

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;

    public UserSyncBehavior(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!IsCommandRequest())
        {
            return await next();
        }

        var principal = _httpContextAccessor.HttpContext?.User;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return await next();
        }

        var userIdClaim = GetFirstClaimValue(principal, UserIdClaimTypes);
        var emailClaim = GetFirstClaimValue(principal, EmailClaimTypes);

        if (!Guid.TryParse(userIdClaim, out var userId) || string.IsNullOrWhiteSpace(emailClaim))
        {
            return await next();
        }

        var userExists = await _context.Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .AnyAsync(u => u.Id == userId, cancellationToken);

        if (!userExists)
        {
            var user = new User
            {
                Id = userId,
                Email = emailClaim,
                Name = BuildDefaultName(emailClaim),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return await next();
    }

    private static string? GetFirstClaimValue(ClaimsPrincipal principal, IEnumerable<string> claimTypes)
    {
        foreach (var claimType in claimTypes)
        {
            var value = principal.FindFirstValue(claimType);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }

    private static bool IsCommandRequest()
    {
        var requestType = typeof(TRequest);
        var namespaceValue = requestType.Namespace;

        return requestType.Name.EndsWith("Command", StringComparison.Ordinal)
            || (namespaceValue is not null && namespaceValue.Contains(".Commands", StringComparison.Ordinal));
    }

    private static string BuildDefaultName(string email)
    {
        var separatorIndex = email.IndexOf('@');
        if (separatorIndex <= 0)
        {
            return email;
        }

        return email[..separatorIndex];
    }
}