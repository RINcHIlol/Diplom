namespace API.Models;

public class UserTaskProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int TaskId { get; set; }
    public Task Task { get; set; }

    public bool IsCorrect { get; set; }
    public DateTime AnsweredAt { get; set; }
}