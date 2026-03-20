namespace API.DTOs.Profile;

public class CourseProgressDto
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public double ProgressPercent { get; set; }

    public List<ModuleProgressDto> Modules { get; set; }
}