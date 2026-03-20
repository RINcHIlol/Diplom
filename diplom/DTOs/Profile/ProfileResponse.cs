using System.Collections.Generic;

namespace API.DTOs.Profile;

public class ProfileResponse
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public int Xp { get; set; }
    public string Role { get; set; }

    public List<CourseProgressDto> Courses { get; set; }
}