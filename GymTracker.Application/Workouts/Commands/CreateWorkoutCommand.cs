using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;

namespace GymTracker.Application.Workouts.Commands;

public sealed class CreateWorkoutCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }

    public Guid? RoutineId { get; init; }

    public IReadOnlyCollection<CreateWorkoutSetItem> Sets { get; init; } = [];
}

public sealed class CreateWorkoutSetItem
{
    public Guid ExerciseId { get; init; }

    public decimal Weight { get; init; }

    public int Reps { get; init; }

    public int? Rir { get; init; }
}

public sealed class CreateWorkoutCommandHandler
    : IRequestHandler<CreateWorkoutCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateWorkoutCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var workoutId = Guid.NewGuid();

        var workoutLog = new WorkoutLog
        {
            Id = workoutId,
            UserId = command.UserId,
            RoutineId = command.RoutineId,
            StartedAt = DateTime.UtcNow,
            ExerciseSets = command.Sets
                .Select((set, index) => new ExerciseSet
                {
                    Id = Guid.NewGuid(),
                    WorkoutLogId = workoutId,
                    ExerciseId = set.ExerciseId,
                    SetNumber = index + 1,
                    Weight = set.Weight,
                    Reps = set.Reps,
                    Rir = set.Rir
                })
                .ToList()
        };

        await _context.WorkoutLogs.AddAsync(workoutLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return workoutLog.Id;
    }
}