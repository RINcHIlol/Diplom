namespace API.Models;

public class Task
{
    public int Id { get; set; }

    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }

    public int TaskTypeId { get; set; }
    public TaskType TaskType { get; set; }

    public string Question { get; set; }
    public int OrderIndex { get; set; }
    public string Content { get; set; }

    public List<TaskAnswer> Answers { get; set; }
}