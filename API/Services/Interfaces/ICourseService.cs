using API.DTOs.Profile;
using API.Models;

namespace API.Services.Interfaces;

public interface ICourseService
{
    // Task<List<Course>> GetAllCoursesAsync();
    Task<List<CourseProgressDto>> GetCoursesAsync(int? userId);
}