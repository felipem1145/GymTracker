using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;

namespace GymTracker.Application.Users.Commands;

public sealed class CreateUserCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;
}

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
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

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            Email = command.Email.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            DeletedAt = null
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
