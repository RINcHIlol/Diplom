namespace API.DTOs.Profile;

public class ModuleProgressDto
{
    public int ModuleId { get; set; }

    public string Title { get; set; }
    public int OrderIndex { get; set; }
    
    public List<LessonProgressDto> Lessons { get; set; }
}