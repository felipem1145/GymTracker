using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Users.Commands;

public sealed class UpdateUserCommand : IRequest<bool>
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;
}

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Name is required.", nameof(command.Name));
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            throw new ArgumentException("Email is required.", nameof(command.Email));
        }

        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

        if (user is null || user.IsDeleted)
        {
            return false;
        }

        user.Name = command.Name.Trim();
        user.Email = command.Email.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
