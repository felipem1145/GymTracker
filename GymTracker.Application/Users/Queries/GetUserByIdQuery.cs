using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Users.Queries;

public sealed class GetUserByIdQuery : IRequest<UserDetailDto?>
{
    public Guid Id { get; init; }
}

public sealed record UserDetailDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt);

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDetailDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == request.Id)
            .Select(u => new UserDetailDto(
                u.Id,
                u.Name,
                u.Email,
                u.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
