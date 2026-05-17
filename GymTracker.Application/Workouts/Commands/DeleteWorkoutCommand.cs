using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Workouts.Commands;

public sealed class DeleteWorkoutCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public sealed class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteWorkoutCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var workout = await _context.WorkoutLogs
            .Include(w => w.ExerciseSets)
            .FirstOrDefaultAsync(w => w.Id == command.Id, cancellationToken);

        if (workout is null)
        {
            return false;
        }

        if (workout.ExerciseSets.Count > 0)
        {
            _context.ExerciseSets.RemoveRange(workout.ExerciseSets);
        }

        _context.WorkoutLogs.Remove(workout);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
