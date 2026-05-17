using GymTracker.Application.Tests.Common;
using GymTracker.Application.Users.Queries;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Users;

public sealed class UserQueriesTests
{
    [Fact]
    public async Task GetUsers_ReturnsOrderedList()
    {
        await using var context = CreateContext();
        context.Users.AddRange(
            new User { Id = Guid.NewGuid(), Name = "Zed", Email = "zed@example.com", CreatedAt = DateTime.UtcNow },
            new User { Id = Guid.NewGuid(), Name = "Ana", Email = "ana@example.com", CreatedAt = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var handler = new GetUsersQueryHandler(context);

        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Ana", result[0].Name);
        Assert.Equal("Zed", result[1].Name);
    }

    [Fact]
    public async Task GetUserById_ReturnsNull_WhenMissing()
    {
        await using var context = CreateContext();
        var handler = new GetUserByIdQueryHandler(context);

        var result = await handler.Handle(new GetUserByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None);

        Assert.Null(result);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
