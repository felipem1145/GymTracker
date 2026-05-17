using GymTracker.Application.Exercises.Commands;
using GymTracker.Application.Exercises.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.WebApi.Controllers;

[ApiController]
[Route("api/exercises")]
public sealed class ExercisesController : ControllerBase
{
    private readonly ISender _sender;

    public ExercisesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExerciseListItemDto>>> Get(CancellationToken cancellationToken)
    {
        var exercises = await _sender.Send(new GetExercisesQuery(), cancellationToken);
        return Ok(exercises);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var exercise = await _sender.Send(new GetExerciseByIdQuery { Id = id }, cancellationToken);

        if (exercise is null)
        {
            return NotFound();
        }

        return Ok(exercise);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateExerciseCommand command, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateExerciseCommand command, CancellationToken cancellationToken)
    {
        var updated = await _sender.Send(new UpdateExerciseCommand
        {
            Id = id,
            Name = command.Name,
            TargetMuscle = command.TargetMuscle
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
        var deleted = await _sender.Send(new DeleteExerciseCommand { Id = id }, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
