using GymTracker.Application.Routines.Queries;
using GymTracker.Application.Tests.Common;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Routines;

public sealed class RoutineQueriesTests
{
    [Fact]
    public async Task GetRoutineById_IncludesExerciseNamesInDto()
    {
        await using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var exercise1 = new Exercise { Id = Guid.NewGuid(), Name = "Squat", TargetMuscle = "Legs" };
        var exercise2 = new Exercise { Id = Guid.NewGuid(), Name = "Deadlift", TargetMuscle = "Back" };

        var routine = new Routine
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Strength",
            CreatedAt = DateTime.UtcNow,
            RoutineExercises =
            [
                new RoutineExercise { RoutineId = Guid.Empty, ExerciseId = exercise2.Id, SequenceOrder = 2 },
                new RoutineExercise { RoutineId = Guid.Empty, ExerciseId = exercise1.Id, SequenceOrder = 1 }
            ]
        };

        routine.RoutineExercises.ElementAt(0).RoutineId = routine.Id;
        routine.RoutineExercises.ElementAt(1).RoutineId = routine.Id;

        context.Users.Add(user);
        context.Exercises.AddRange(exercise1, exercise2);
        context.Routines.Add(routine);
        await context.SaveChangesAsync();

        var handler = new GetRoutineByIdQueryHandler(context, new TestCurrentUserService(user.Id));

        var result = await handler.Handle(new GetRoutineByIdQuery { Id = routine.Id }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Exercises.Count);
        Assert.Equal("Squat", result.Exercises[0].ExerciseName);
        Assert.Equal("Deadlift", result.Exercises[1].ExerciseName);
    }

    [Fact]
    public async Task GetRoutines_ReturnsExerciseNamesInList()
    {
        await using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var exercise = new Exercise { Id = Guid.NewGuid(), Name = "Bench Press", TargetMuscle = "Chest" };

        var routine = new Routine
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Push",
            CreatedAt = DateTime.UtcNow,
            RoutineExercises =
            [
                new RoutineExercise { RoutineId = Guid.Empty, ExerciseId = exercise.Id, SequenceOrder = 1 }
            ]
        };

        routine.RoutineExercises.ElementAt(0).RoutineId = routine.Id;

        context.Users.Add(user);
        context.Exercises.Add(exercise);
        context.Routines.Add(routine);
        await context.SaveChangesAsync();

        var handler = new GetRoutinesQueryHandler(context, new TestCurrentUserService(user.Id));
        var result = await handler.Handle(new GetRoutinesQuery(), CancellationToken.None);

        Assert.Single(result);
        Assert.Single(result[0].Exercises);
        Assert.Equal("Bench Press", result[0].Exercises[0].ExerciseName);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
