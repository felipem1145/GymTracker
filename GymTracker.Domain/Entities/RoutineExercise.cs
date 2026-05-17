namespace GymTracker.Domain.Entities;

public class RoutineExercise
{
    public Guid RoutineId { get; set; }
    public Guid ExerciseId { get; set; }
    public int SequenceOrder { get; set; }

    // Navigation properties
    public virtual Routine? Routine { get; set; }
    public virtual Exercise? Exercise { get; set; }
}
