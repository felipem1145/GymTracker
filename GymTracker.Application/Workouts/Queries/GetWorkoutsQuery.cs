using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Workouts.Queries;

public sealed class GetWorkoutsQuery : IRequest<IReadOnlyList<WorkoutListItemDto>>
{
}

public sealed record WorkoutSetDto(
    Guid ExerciseId,
    string ExerciseName,
    int SetNumber,
    decimal Weight,
    int Reps,
    int? Rir);

public sealed record WorkoutListItemDto(
    Guid Id,
    Guid UserId,
    Guid? RoutineId,
    DateTime StartedAt,
    IReadOnlyList<WorkoutSetDto> Sets);

public sealed class GetWorkoutsQueryHandler
    : IRequestHandler<GetWorkoutsQuery, IReadOnlyList<WorkoutListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetWorkoutsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<WorkoutListItemDto>> Handle(
        GetWorkoutsQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId is null)
        {
            return [];
        }

        return await _context.WorkoutLogs
            .AsNoTracking()
            .Where(w => w.UserId == currentUserId.Value)
            .OrderByDescending(w => w.StartedAt)
            .Select(w => new WorkoutListItemDto(
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
            .ToListAsync(cancellationToken);
    }
}
