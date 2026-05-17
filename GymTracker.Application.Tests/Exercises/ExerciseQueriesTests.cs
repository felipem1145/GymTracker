using GymTracker.Application.Exercises.Queries;
using GymTracker.Application.Tests.Common;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Exercises;

public sealed class ExerciseQueriesTests
{
    [Fact]
    public async Task GetExercises_ReturnsOrderedList()
    {
        await using var context = CreateContext();
        context.Exercises.AddRange(
            new Exercise { Id = Guid.NewGuid(), Name = "Press", TargetMuscle = "Chest" },
            new Exercise { Id = Guid.NewGuid(), Name = "Curl", TargetMuscle = "Biceps" });
        await context.SaveChangesAsync();

        var handler = new GetExercisesQueryHandler(context);

        var result = await handler.Handle(new GetExercisesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Curl", result[0].Name);
        Assert.Equal("Press", result[1].Name);
    }

    [Fact]
    public async Task GetExerciseById_ReturnsEntity_WhenExists()
    {
        await using var context = CreateContext();
        var exercise = new Exercise { Id = Guid.NewGuid(), Name = "Row", TargetMuscle = "Back" };
        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        var handler = new GetExerciseByIdQueryHandler(context);

        var result = await handler.Handle(new GetExerciseByIdQuery { Id = exercise.Id }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Row", result!.Name);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
