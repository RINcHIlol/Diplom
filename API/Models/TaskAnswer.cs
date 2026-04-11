namespace API.Models;

public class TaskAnswer
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public Task Task { get; set; }

    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}