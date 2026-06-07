using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Routines.Commands;

public sealed class UpdateRoutineCommand : IRequest<bool>
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public IReadOnlyCollection<Guid> ExerciseIds { get; init; } = [];
}

public sealed class UpdateRoutineCommandHandler : IRequestHandler<UpdateRoutineCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateRoutineCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UpdateRoutineCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Authenticated user id is required.");

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Name is required.", nameof(command.Name));
        }

        var routine = await _context.Routines
            .IgnoreQueryFilters()
            .Include(r => r.RoutineExercises)
            .FirstOrDefaultAsync(r => r.Id == command.Id && r.UserId == userId, cancellationToken);

        if (routine is null || routine.IsDeleted)
        {
            return false;
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

        routine.Name = command.Name.Trim();

        // Synchronize join rows: remove missing, add new, and re-sequence by input order.
        var toRemove = routine.RoutineExercises
            .Where(re => !normalizedExerciseIds.Contains(re.ExerciseId))
            .ToList();

        if (toRemove.Count > 0)
        {
            _context.RoutineExercises.RemoveRange(toRemove);
        }

        var existingByExerciseId = routine.RoutineExercises
            .Where(re => !toRemove.Contains(re))
            .ToDictionary(re => re.ExerciseId, re => re);

        for (var index = 0; index < normalizedExerciseIds.Count; index++)
        {
            var exerciseId = normalizedExerciseIds[index];
            var sequenceOrder = index + 1;

            if (existingByExerciseId.TryGetValue(exerciseId, out var existingRoutineExercise))
            {
                existingRoutineExercise.SequenceOrder = sequenceOrder;
                continue;
            }

            routine.RoutineExercises.Add(new RoutineExercise
            {
                RoutineId = routine.Id,
                ExerciseId = exerciseId,
                SequenceOrder = sequenceOrder
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
