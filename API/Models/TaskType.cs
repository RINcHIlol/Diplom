namespace API.Models;

public class TaskType
{
    public int Id { get; set; }
    public string Title { get; set; }

    public List<Task> Tasks { get; set; }
}