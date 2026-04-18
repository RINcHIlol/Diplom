using API.DTOs.Profile;
using API.Models;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories.Interfaces;

public interface ICourseRepository
{
    // Task<List<Course>> GetAllAsync();
    Task<List<CourseProgressDto>> GetCoursesForUserAsync(int? userId);
    Task<List<CourseShortDto>> GetMyCoursesAsync(int userId);
    Task<Course> CreateAsync(Course course);
    Task<Course?> GetByIdAsync(int id);
    Task UpdateAsync(Course course);
    Task<bool> IsOwnerAsync(int courseId, int userId);
    Task DeleteAsync(int courseId);
}