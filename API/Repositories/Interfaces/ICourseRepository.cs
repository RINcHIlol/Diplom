using API.DTOs.Profile;
using API.Models;

namespace API.Repositories.Interfaces;

public interface ICourseRepository
{
    // Task<List<Course>> GetAllAsync();
    Task<List<CourseProgressDto>> GetCoursesForUserAsync(int? userId);
}