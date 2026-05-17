using GymTracker.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Routines.Commands;

public sealed class DeleteRoutineCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public sealed class DeleteRoutineCommandHandler : IRequestHandler<DeleteRoutineCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoutineCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteRoutineCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var routine = await _context.Routines
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

        if (routine is null || routine.IsDeleted)
        {
            return false;
        }

        routine.IsDeleted = true;
        routine.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
