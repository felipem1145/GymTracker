using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Application.Tests.Common;

public sealed class TestApplicationDbContext : DbContext, IApplicationDbContext
{
    public TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Routine> Routines => Set<Routine>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<RoutineExercise> RoutineExercises => Set<RoutineExercise>();
    public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();
    public DbSet<ExerciseSet> ExerciseSets => Set<ExerciseSet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoutineExercise>()
            .HasKey(re => new { re.RoutineId, re.ExerciseId });

        modelBuilder.Entity<RoutineExercise>()
            .HasOne(re => re.Routine)
            .WithMany(r => r.RoutineExercises)
            .HasForeignKey(re => re.RoutineId);

        modelBuilder.Entity<RoutineExercise>()
            .HasOne(re => re.Exercise)
            .WithMany(e => e.RoutineExercises)
            .HasForeignKey(re => re.ExerciseId);
    }
}
