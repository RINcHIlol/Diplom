using diplom.DTOs.Profile;

namespace diplom.Services;

public class NavigationService
{
    public int? CurrentCourseId { get; set; }
    public int? CurrentModuleId { get; set; }
    public int? CurrentLessonId { get; set; }
    public int? CurrentTaskId { get; set; }
}