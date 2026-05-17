using GymTracker.Application.Exercises.Commands;
using GymTracker.Application.Tests.Common;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Exercises;

public sealed class ExerciseCommandsTests
{
    [Fact]
    public async Task DeleteExercise_SetsSoftDeleteFields_WhenExerciseExists()
    {
        await using var context = CreateContext();
        var exercise = new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Bench Press",
            TargetMuscle = "Chest"
        };

        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        var handler = new DeleteExerciseCommandHandler(context);

        var deleted = await handler.Handle(new DeleteExerciseCommand { Id = exercise.Id });

        Assert.True(deleted);
        Assert.True(exercise.IsDeleted);
        Assert.NotNull(exercise.DeletedAt);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
