using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Users.Commands;

public sealed class DeleteUserCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

        if (user is null || user.IsDeleted)
        {
            return false;
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
