using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Routines.Queries;

public sealed class GetRoutinesQuery : IRequest<IReadOnlyList<RoutineListItemDto>>
{
}

public sealed record RoutineExerciseDto(
    Guid ExerciseId,
    string ExerciseName,
    int SequenceOrder);

public sealed record RoutineListItemDto(
    Guid Id,
    Guid UserId,
    string Name,
    DateTime CreatedAt,
    IReadOnlyList<RoutineExerciseDto> Exercises);

public sealed class GetRoutinesQueryHandler
    : IRequestHandler<GetRoutinesQuery, IReadOnlyList<RoutineListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetRoutinesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<RoutineListItemDto>> Handle(
        GetRoutinesQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId is null)
        {
            return [];
        }

        return await _context.Routines
            .AsNoTracking()
            .Where(r => r.UserId == currentUserId.Value)
            .OrderBy(r => r.Name)
            .Select(r => new RoutineListItemDto(
                r.Id,
                r.UserId,
                r.Name,
                r.CreatedAt,
                r.RoutineExercises
                    .OrderBy(re => re.SequenceOrder)
                    .Select(re => new RoutineExerciseDto(
                        re.ExerciseId,
                        re.Exercise != null ? re.Exercise.Name : string.Empty,
                        re.SequenceOrder))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }
}
