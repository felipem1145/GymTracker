using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Routines.Queries;

public sealed class GetRoutineByIdQuery : IRequest<RoutineDetailDto?>
{
    public Guid Id { get; init; }
}

public sealed record RoutineDetailDto(
    Guid Id,
    Guid UserId,
    string Name,
    DateTime CreatedAt,
    IReadOnlyList<RoutineExerciseDto> Exercises);

public sealed class GetRoutineByIdQueryHandler : IRequestHandler<GetRoutineByIdQuery, RoutineDetailDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetRoutineByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<RoutineDetailDto?> Handle(
        GetRoutineByIdQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId is null)
        {
            return null;
        }

        return await _context.Routines
            .AsNoTracking()
            .Where(r => r.Id == request.Id && r.UserId == currentUserId.Value)
            .Select(r => new RoutineDetailDto(
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}
