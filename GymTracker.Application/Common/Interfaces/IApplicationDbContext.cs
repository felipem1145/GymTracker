using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    DbSet<Routine> Routines { get; }

    DbSet<Exercise> Exercises { get; }

    DbSet<RoutineExercise> RoutineExercises { get; }

    DbSet<WorkoutLog> WorkoutLogs { get; }

    DbSet<ExerciseSet> ExerciseSets { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}