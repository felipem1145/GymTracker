using GymTracker.Application.Routines.Commands;
using GymTracker.Application.Routines.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.WebApi.Controllers;

[ApiController]
[Route("api/routines")]
public sealed class RoutinesController : ControllerBase
{
    private readonly ISender _sender;

    public RoutinesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoutineListItemDto>>> Get(CancellationToken cancellationToken)
    {
        var routines = await _sender.Send(new GetRoutinesQuery(), cancellationToken);
        return Ok(routines);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoutineDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var routine = await _sender.Send(new GetRoutineByIdQuery { Id = id }, cancellationToken);

        if (routine is null)
        {
            return NotFound();
        }

        return Ok(routine);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateRoutineCommand command, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateRoutineCommand command, CancellationToken cancellationToken)
    {
        var updated = await _sender.Send(new UpdateRoutineCommand
        {
            Id = id,
            UserId = command.UserId,
            Name = command.Name,
            ExerciseIds = command.ExerciseIds
        }, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(new DeleteRoutineCommand { Id = id }, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
