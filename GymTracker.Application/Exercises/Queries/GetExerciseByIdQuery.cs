using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Exercises.Queries;

public sealed class GetExerciseByIdQuery : IRequest<ExerciseDetailDto?>
{
    public Guid Id { get; init; }
}

public sealed record ExerciseDetailDto(
    Guid Id,
    string Name,
    string TargetMuscle);

public sealed class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetExerciseByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDetailDto?> Handle(
        GetExerciseByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Exercises
            .AsNoTracking()
            .Where(e => e.Id == request.Id)
            .Select(e => new ExerciseDetailDto(
                e.Id,
                e.Name,
                e.TargetMuscle))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
