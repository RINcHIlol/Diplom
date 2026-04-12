using API.DTOs.Profile;
using API.Models;

namespace API.Services.Interfaces;

public interface ICourseService
{
    // Task<List<Course>> GetAllCoursesAsync();
    Task<List<CourseProgressDto>> GetCoursesAsync(int? userId);
    Task<List<CourseShortDto>> GetCoursesForCreatorAsync(int userId);
    Task<CourseShortDto> GetCourseAsync(int courseId);
    Task<Course> CreateCourseAsync(int userId, CreateUpdateCourseDto dto);
    Task<bool> UpdateCourseAsync(int userId, int courseId, CreateUpdateCourseDto dto);
}