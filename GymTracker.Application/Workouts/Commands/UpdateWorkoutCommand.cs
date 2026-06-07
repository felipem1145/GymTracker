using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Workouts.Commands;

public sealed class UpdateWorkoutCommand : IRequest<bool>
{
    public Guid Id { get; init; }

    public Guid? RoutineId { get; init; }

    public IReadOnlyCollection<UpdateWorkoutSetItem> Sets { get; init; } = [];
}

public sealed class UpdateWorkoutSetItem
{
    public Guid ExerciseId { get; init; }

    public decimal Weight { get; init; }

    public int Reps { get; init; }

    public int? Rir { get; init; }
}

public sealed class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateWorkoutCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UpdateWorkoutCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Authenticated user id is required.");

        var workout = await _context.WorkoutLogs
            .Include(w => w.ExerciseSets)
            .FirstOrDefaultAsync(w => w.Id == command.Id && w.UserId == userId, cancellationToken);

        if (workout is null)
        {
            return false;
        }

        if (command.RoutineId.HasValue)
        {
            var routineExists = await _context.Routines
                .AnyAsync(r => r.Id == command.RoutineId.Value && r.UserId == userId, cancellationToken);

            if (!routineExists)
            {
                throw new InvalidOperationException("Routine was not found.");
            }
        }

        var normalizedExerciseIds = command.Sets
            .Select(s => s.ExerciseId)
            .Distinct()
            .ToList();

        if (normalizedExerciseIds.Count > 0)
        {
            var existingExercisesCount = await _context.Exercises
                .CountAsync(e => normalizedExerciseIds.Contains(e.Id), cancellationToken);

            if (existingExercisesCount != normalizedExerciseIds.Count)
            {
                throw new InvalidOperationException("One or more ExerciseIds were not found.");
            }
        }

        workout.RoutineId = command.RoutineId;

        if (workout.ExerciseSets.Count > 0)
        {
            _context.ExerciseSets.RemoveRange(workout.ExerciseSets);
        }

        workout.ExerciseSets = command.Sets
            .Select((set, index) => new ExerciseSet
            {
                Id = Guid.NewGuid(),
                WorkoutLogId = workout.Id,
                ExerciseId = set.ExerciseId,
                SetNumber = index + 1,
                Weight = set.Weight,
                Reps = set.Reps,
                Rir = set.Rir
            })
            .ToList();

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
