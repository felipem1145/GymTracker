using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;

namespace GymTracker.Application.Exercises.Commands;

public sealed class CreateExerciseCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;

    public string TargetMuscle { get; init; } = string.Empty;
}

public sealed class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateExerciseCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Name is required.", nameof(command.Name));
        }

        if (string.IsNullOrWhiteSpace(command.TargetMuscle))
        {
            throw new ArgumentException("TargetMuscle is required.", nameof(command.TargetMuscle));
        }

        var exercise = new Exercise
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            TargetMuscle = command.TargetMuscle.Trim(),
            IsDeleted = false,
            DeletedAt = null
        };

        await _context.Exercises.AddAsync(exercise, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return exercise.Id;
    }
}
