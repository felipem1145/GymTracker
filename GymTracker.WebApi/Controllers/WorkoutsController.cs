using GymTracker.Application.Workouts.Commands;
using GymTracker.Application.Workouts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.WebApi.Controllers;

[ApiController]
[Route("api/workouts")]
public sealed class WorkoutsController : ControllerBase
{
    private readonly ISender _sender;

    public WorkoutsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WorkoutListItemDto>>> Get(CancellationToken cancellationToken)
    {
        var workouts = await _sender.Send(new GetWorkoutsQuery(), cancellationToken);
        return Ok(workouts);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var workout = await _sender.Send(new GetWorkoutByIdQuery { Id = id }, cancellationToken);

        if (workout is null)
        {
            return NotFound();
        }

        return Ok(workout);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateWorkoutCommand command, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateWorkoutCommand command, CancellationToken cancellationToken)
    {
        var updated = await _sender.Send(new UpdateWorkoutCommand
        {
            Id = id,
            UserId = command.UserId,
            RoutineId = command.RoutineId,
            Sets = command.Sets
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
        var deleted = await _sender.Send(new DeleteWorkoutCommand { Id = id }, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
