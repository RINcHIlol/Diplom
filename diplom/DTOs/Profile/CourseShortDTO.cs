using System;

namespace diplom.DTOs.Profile;

public class CourseShortDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[]? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}