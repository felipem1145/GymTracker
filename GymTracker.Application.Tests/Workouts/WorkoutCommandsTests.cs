using GymTracker.Application.Tests.Common;
using GymTracker.Application.Workouts.Commands;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Workouts;

public sealed class WorkoutCommandsTests
{
    [Fact]
    public async Task UpdateWorkout_ReturnsFalse_WhenWorkoutDoesNotExist()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new UpdateWorkoutCommandHandler(context, new TestCurrentUserService(user.Id));

        var result = await handler.Handle(new UpdateWorkoutCommand
        {
            Id = Guid.NewGuid(),
            Sets = []
        });

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteWorkout_RemovesWorkoutAndSets_WhenExists()
    {
        await using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var exercise = new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Row",
            TargetMuscle = "Back"
        };

        var workout = new WorkoutLog
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            StartedAt = DateTime.UtcNow,
            ExerciseSets =
            [
                new ExerciseSet
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = exercise.Id,
                    SetNumber = 1,
                    Weight = 60,
                    Reps = 10
                }
            ]
        };
        workout.ExerciseSets.ElementAt(0).WorkoutLogId = workout.Id;

        context.Users.Add(user);
        context.Exercises.Add(exercise);
        context.WorkoutLogs.Add(workout);
        await context.SaveChangesAsync();

        var handler = new DeleteWorkoutCommandHandler(context);
        var deleted = await handler.Handle(new DeleteWorkoutCommand { Id = workout.Id });

        Assert.True(deleted);
        Assert.Empty(context.WorkoutLogs);
        Assert.Empty(context.ExerciseSets);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
