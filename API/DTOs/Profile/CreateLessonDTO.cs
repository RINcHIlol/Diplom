namespace API.DTOs.Profile;

public class CreateUpdateLessonDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int OrderIndex { get; set; }
}