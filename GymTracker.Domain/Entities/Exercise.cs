namespace GymTracker.Domain.Entities;

public class Exercise
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TargetMuscle { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<RoutineExercise> RoutineExercises { get; set; } = new List<RoutineExercise>();
    public virtual ICollection<ExerciseSet> ExerciseSets { get; set; } = new List<ExerciseSet>();
}
