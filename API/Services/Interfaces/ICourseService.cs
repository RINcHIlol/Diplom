using API.Models;

namespace API.Services.Interfaces;

public interface ICourseService
{
    Task<List<Course>> GetAllCoursesAsync();
}