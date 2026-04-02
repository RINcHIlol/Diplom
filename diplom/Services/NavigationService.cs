using diplom.DTOs.Profile;

namespace diplom.Services;

public class NavigationService
{
    public CourseProgressDto? CurrentCourse { get; set; }
    public ModuleProgressDto? CurrentModule { get; set; }
}