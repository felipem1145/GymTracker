namespace GymTracker.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();
    public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
}
