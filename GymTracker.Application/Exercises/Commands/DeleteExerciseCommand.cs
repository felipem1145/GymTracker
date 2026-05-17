using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Exercises.Commands;

public sealed class DeleteExerciseCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public sealed class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteExerciseCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var exercise = await _context.Exercises
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        if (exercise is null || exercise.IsDeleted)
        {
            return false;
        }

        exercise.IsDeleted = true;
        exercise.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
