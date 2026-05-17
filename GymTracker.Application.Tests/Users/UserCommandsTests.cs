using GymTracker.Application.Tests.Common;
using GymTracker.Application.Users.Commands;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Users;

public sealed class UserCommandsTests
{
    [Fact]
    public async Task UpdateUser_ReturnsFalse_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();
        var handler = new UpdateUserCommandHandler(context);

        var result = await handler.Handle(new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated",
            Email = "updated@example.com"
        });

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUser_SetsSoftDeleteFields_WhenUserExists()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new DeleteUserCommandHandler(context);

        var deleted = await handler.Handle(new DeleteUserCommand { Id = user.Id });

        Assert.True(deleted);
        Assert.True(user.IsDeleted);
        Assert.NotNull(user.DeletedAt);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
