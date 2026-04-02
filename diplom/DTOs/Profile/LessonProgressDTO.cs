namespace diplom.DTOs.Profile;

public class LessonProgressDto
{
    public int LessonId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    
    public int OrderIndex { get; set; }
}