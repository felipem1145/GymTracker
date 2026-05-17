using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Exercises.Commands;

public sealed class UpdateExerciseCommand : IRequest<bool>
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string TargetMuscle { get; init; } = string.Empty;
}

public sealed class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateExerciseCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("Name is required.", nameof(command.Name));
        }

        if (string.IsNullOrWhiteSpace(command.TargetMuscle))
        {
            throw new ArgumentException("TargetMuscle is required.", nameof(command.TargetMuscle));
        }

        var exercise = await _context.Exercises
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        if (exercise is null || exercise.IsDeleted)
        {
            return false;
        }

        exercise.Name = command.Name.Trim();
        exercise.TargetMuscle = command.TargetMuscle.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
