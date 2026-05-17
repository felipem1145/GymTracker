using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Exercises.Queries;

public sealed class GetExercisesQuery : IRequest<IReadOnlyList<ExerciseListItemDto>>
{
}

public sealed record ExerciseListItemDto(
    Guid Id,
    string Name,
    string TargetMuscle);

public sealed class GetExercisesQueryHandler
    : IRequestHandler<GetExercisesQuery, IReadOnlyList<ExerciseListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetExercisesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ExerciseListItemDto>> Handle(
        GetExercisesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .AsNoTracking()
            .OrderBy(e => e.Name)
            .Select(e => new ExerciseListItemDto(
                e.Id,
                e.Name,
                e.TargetMuscle))
            .ToListAsync(cancellationToken);
    }
}
