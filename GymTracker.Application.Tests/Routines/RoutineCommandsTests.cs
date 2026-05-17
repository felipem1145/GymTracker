using GymTracker.Application.Routines.Commands;
using GymTracker.Application.Tests.Common;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GymTracker.Application.Tests.Routines;

public sealed class RoutineCommandsTests
{
    [Fact]
    public async Task CreateRoutine_Throws_WhenAnyExerciseIdDoesNotExist()
    {
        await using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var existingExercise = new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Squat",
            TargetMuscle = "Legs"
        };

        context.Users.Add(user);
        context.Exercises.Add(existingExercise);
        await context.SaveChangesAsync();

        var handler = new CreateRoutineCommandHandler(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new CreateRoutineCommand
        {
            UserId = user.Id,
            Name = "Leg Day",
            ExerciseIds = [existingExercise.Id, Guid.NewGuid()]
        }));
    }

    [Fact]
    public async Task UpdateRoutine_Synchronizes_RoutineExercises()
    {
        await using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@example.com",
            CreatedAt = DateTime.UtcNow
        };

        var exercise1 = new Exercise { Id = Guid.NewGuid(), Name = "A", TargetMuscle = "X" };
        var exercise2 = new Exercise { Id = Guid.NewGuid(), Name = "B", TargetMuscle = "Y" };
        var exercise3 = new Exercise { Id = Guid.NewGuid(), Name = "C", TargetMuscle = "Z" };

        var routine = new Routine
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Initial",
            CreatedAt = DateTime.UtcNow,
            RoutineExercises =
            [
                new RoutineExercise { RoutineId = Guid.Empty, ExerciseId = exercise1.Id, SequenceOrder = 1 },
                new RoutineExercise { RoutineId = Guid.Empty, ExerciseId = exercise2.Id, SequenceOrder = 2 }
            ]
        };
        routine.RoutineExercises.ElementAt(0).RoutineId = routine.Id;
        routine.RoutineExercises.ElementAt(1).RoutineId = routine.Id;

        context.Users.Add(user);
        context.Exercises.AddRange(exercise1, exercise2, exercise3);
        context.Routines.Add(routine);
        await context.SaveChangesAsync();

        var handler = new UpdateRoutineCommandHandler(context);

        var updated = await handler.Handle(new UpdateRoutineCommand
        {
            Id = routine.Id,
            UserId = user.Id,
            Name = "Updated",
            ExerciseIds = [exercise2.Id, exercise3.Id]
        });

        Assert.True(updated);

        var routineExercises = await context.RoutineExercises
            .Where(re => re.RoutineId == routine.Id)
            .OrderBy(re => re.SequenceOrder)
            .ToListAsync();

        Assert.Equal(2, routineExercises.Count);
        Assert.Equal(exercise2.Id, routineExercises[0].ExerciseId);
        Assert.Equal(1, routineExercises[0].SequenceOrder);
        Assert.Equal(exercise3.Id, routineExercises[1].ExerciseId);
        Assert.Equal(2, routineExercises[1].SequenceOrder);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
