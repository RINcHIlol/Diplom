namespace API.DTOs.Profile;

public class CourseProgressDto
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] Image { get; set; }
    public DateTime CreatedAt { get; set; } 
    public double ProgressPercent { get; set; }

    public List<ModuleProgressDto> Modules { get; set; }
}