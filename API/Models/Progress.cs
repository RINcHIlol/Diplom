namespace API.Models;

public class UserProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
}