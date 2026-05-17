using GymTracker.Application.Users.Commands;
using GymTracker.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserListItemDto>>> Get(CancellationToken cancellationToken)
    {
        var users = await _sender.Send(new GetUsersQuery(), cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _sender.Send(new GetUserByIdQuery { Id = id }, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Post([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var updated = await _sender.Send(new UpdateUserCommand
        {
            Id = id,
            Name = command.Name,
            Email = command.Email
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
        var deleted = await _sender.Send(new DeleteUserCommand { Id = id }, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
