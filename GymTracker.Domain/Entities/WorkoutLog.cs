namespace GymTracker.Domain.Entities;

public class WorkoutLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? RoutineId { get; set; }
    public DateTime StartedAt { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Routine? Routine { get; set; }
    public virtual ICollection<ExerciseSet> ExerciseSets { get; set; } = new List<ExerciseSet>();
}
