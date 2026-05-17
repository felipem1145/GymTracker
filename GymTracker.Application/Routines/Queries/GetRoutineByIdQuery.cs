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

    public GetRoutineByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoutineDetailDto?> Handle(
        GetRoutineByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Routines
            .AsNoTracking()
            .Where(r => r.Id == request.Id)
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
