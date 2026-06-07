using GymTracker.Application.Tests.Common;
using GymTracker.Application.Workouts.Queries;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Workouts;

public sealed class WorkoutQueriesTests
{
    [Fact]
    public async Task GetWorkoutById_ReturnsSetDataWithExerciseName()
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
            Name = "Press",
            TargetMuscle = "Chest"
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
                    Weight = 80,
                    Reps = 8,
                    Rir = 2
                }
            ]
        };
        workout.ExerciseSets.ElementAt(0).WorkoutLogId = workout.Id;

        context.Users.Add(user);
        context.Exercises.Add(exercise);
        context.WorkoutLogs.Add(workout);
        await context.SaveChangesAsync();

        var handler = new GetWorkoutByIdQueryHandler(context, new TestCurrentUserService(user.Id));
        var result = await handler.Handle(new GetWorkoutByIdQuery { Id = workout.Id }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result!.Sets);
        Assert.Equal("Press", result.Sets[0].ExerciseName);
        Assert.Equal(80, result.Sets[0].Weight);
    }

    [Fact]
    public async Task GetWorkouts_ReturnsMostRecentFirst()
    {
        await using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var oldWorkout = new WorkoutLog
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            StartedAt = DateTime.UtcNow.AddDays(-1)
        };

        var newWorkout = new WorkoutLog
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            StartedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        context.WorkoutLogs.AddRange(oldWorkout, newWorkout);
        await context.SaveChangesAsync();

        var handler = new GetWorkoutsQueryHandler(context, new TestCurrentUserService(user.Id));
        var result = await handler.Handle(new GetWorkoutsQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(newWorkout.Id, result[0].Id);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
