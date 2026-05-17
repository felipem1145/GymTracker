using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Workouts.Queries;

public sealed class GetWorkoutByIdQuery : IRequest<WorkoutDetailDto?>
{
    public Guid Id { get; init; }
}

public sealed record WorkoutDetailDto(
    Guid Id,
    Guid UserId,
    Guid? RoutineId,
    DateTime StartedAt,
    IReadOnlyList<WorkoutSetDto> Sets);

public sealed class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetWorkoutByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutDetailDto?> Handle(
        GetWorkoutByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.WorkoutLogs
            .AsNoTracking()
            .Where(w => w.Id == request.Id)
            .Select(w => new WorkoutDetailDto(
                w.Id,
                w.UserId,
                w.RoutineId,
                w.StartedAt,
                w.ExerciseSets
                    .OrderBy(s => s.SetNumber)
                    .Select(s => new WorkoutSetDto(
                        s.ExerciseId,
                        s.Exercise != null ? s.Exercise.Name : string.Empty,
                        s.SetNumber,
                        s.Weight,
                        s.Reps,
                        s.Rir))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
