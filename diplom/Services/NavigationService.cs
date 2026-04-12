using diplom.DTOs.Profile;

namespace diplom.Services;

// public class NavigationService
// {
//     public CourseProgressDto? CurrentCourse { get; set; }
//     public ModuleProgressDto? CurrentModule { get; set; }
//     public LessonProgressDto? CurrentLesson { get; set; }
// }

public class NavigationService
{
    public int? CurrentCourseId { get; set; }
    public int? CurrentModuleId { get; set; }
    public int? CurrentLessonId { get; set; }
}