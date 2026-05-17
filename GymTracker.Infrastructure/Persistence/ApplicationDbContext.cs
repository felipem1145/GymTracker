using GymTracker.Application.Common.Interfaces;
using GymTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

        // ── User ──────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // ── Routine ───────────────────────────────────────────
        modelBuilder.Entity<Routine>(entity =>
        {
            entity.ToTable("routines");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Routines)
                  .HasForeignKey(e => e.UserId);
        });

        // ── Exercise ──────────────────────────────────────────
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.ToTable("exercises");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.TargetMuscle).HasColumnName("target_muscle");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // ── RoutineExercise (join table) ──────────────────────
        modelBuilder.Entity<RoutineExercise>(entity =>
        {
            entity.ToTable("routine_exercises");

            entity.HasKey(re => new { re.RoutineId, re.ExerciseId });

            entity.Property(e => e.RoutineId).HasColumnName("routine_id");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.SequenceOrder).HasColumnName("sequence_order");

            entity.HasOne(e => e.Routine)
                  .WithMany(r => r.RoutineExercises)
                  .HasForeignKey(e => e.RoutineId);

            entity.HasOne(e => e.Exercise)
                  .WithMany(ex => ex.RoutineExercises)
                  .HasForeignKey(e => e.ExerciseId);
        });

        // ── WorkoutLog ────────────────────────────────────────
        modelBuilder.Entity<WorkoutLog>(entity =>
        {
            entity.ToTable("workout_logs");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoutineId).HasColumnName("routine_id");
            entity.Property(e => e.StartedAt).HasColumnName("started_at");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.WorkoutLogs)
                  .HasForeignKey(e => e.UserId);

            entity.HasOne(e => e.Routine)
                  .WithMany(r => r.WorkoutLogs)
                  .HasForeignKey(e => e.RoutineId)
                  .IsRequired(false);
        });

        // ── ExerciseSet ───────────────────────────────────────
        modelBuilder.Entity<ExerciseSet>(entity =>
        {
            entity.ToTable("exercise_sets");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.WorkoutLogId).HasColumnName("workout_log_id");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.SetNumber).HasColumnName("set_number");
            entity.Property(e => e.Weight).HasColumnName("weight").HasPrecision(6, 2);
            entity.Property(e => e.Reps).HasColumnName("reps");
            entity.Property(e => e.Rir).HasColumnName("rir");

            entity.HasOne(e => e.WorkoutLog)
                  .WithMany(w => w.ExerciseSets)
                  .HasForeignKey(e => e.WorkoutLogId);

            entity.HasOne(e => e.Exercise)
                  .WithMany(ex => ex.ExerciseSets)
                  .HasForeignKey(e => e.ExerciseId);
        });
    }
}
