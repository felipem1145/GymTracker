using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Routines.Commands;

public sealed class CreateRoutineCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public IReadOnlyCollection<Guid> ExerciseIds { get; init; } = [];
}

public sealed class CreateRoutineCommandHandler : IRequestHandler<CreateRoutineCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateRoutineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRoutineCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Name is required.", nameof(command.Name));
        }

        var userExists = await _context.Users
            .AnyAsync(u => u.Id == command.UserId, cancellationToken);

        if (!userExists)
        {
            throw new InvalidOperationException("User was not found.");
        }

        var normalizedExerciseIds = command.ExerciseIds.Distinct().ToList();
        if (normalizedExerciseIds.Count != command.ExerciseIds.Count)
        {
            throw new ArgumentException("ExerciseIds contains duplicates.", nameof(command.ExerciseIds));
        }

        if (normalizedExerciseIds.Count > 0)
        {
            var existingExercisesCount = await _context.Exercises
                .CountAsync(e => normalizedExerciseIds.Contains(e.Id), cancellationToken);

            if (existingExercisesCount != normalizedExerciseIds.Count)
            {
                throw new InvalidOperationException("One or more ExerciseIds were not found.");
            }
        }

        var routineId = Guid.NewGuid();
        var routine = new Routine
        {
            Id = routineId,
            UserId = command.UserId,
            Name = command.Name.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            DeletedAt = null,
            RoutineExercises = normalizedExerciseIds
                .Select((exerciseId, index) => new RoutineExercise
                {
                    RoutineId = routineId,
                    ExerciseId = exerciseId,
                    SequenceOrder = index + 1
                })
                .ToList()
        };

        await _context.Routines.AddAsync(routine, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return routine.Id;
    }
}
