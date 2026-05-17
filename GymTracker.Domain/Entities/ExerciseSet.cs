namespace GymTracker.Domain.Entities;

public class ExerciseSet
{
    public Guid Id { get; set; }
    public Guid WorkoutLogId { get; set; }
    public Guid ExerciseId { get; set; }
    public int SetNumber { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public int? Rir { get; set; }

    // Navigation properties
    public virtual WorkoutLog? WorkoutLog { get; set; }
    public virtual Exercise? Exercise { get; set; }
}
