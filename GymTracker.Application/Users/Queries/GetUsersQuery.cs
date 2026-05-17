using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Users.Queries;

public sealed class GetUsersQuery : IRequest<IReadOnlyList<UserListItemDto>>
{
}

public sealed record UserListItemDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt);

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, IReadOnlyList<UserListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserListItemDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .Select(u => new UserListItemDto(
                u.Id,
                u.Name,
                u.Email,
                u.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
