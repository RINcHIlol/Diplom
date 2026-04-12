namespace API.DTOs.Profile;

public class CreateUpdateCourseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[]? Image { get; set; }
}