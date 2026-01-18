using API.Models;
using API.Repositories;

namespace API.Services;

public interface ICourseService
{
    Task<List<Course>> GetAllCoursesAsync();
    // Task<Course> GetCourseAsync(int id);
    // Task AddCourseAsync(Course course);
}

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    public CourseService(ICourseRepository repository) => _repository = repository;

    public Task<List<Course>> GetAllCoursesAsync() => _repository.GetAllAsync();
    // public Task<Course> GetCourseAsync(int id) => _repository.GetByIdAsync(id);
    // public Task AddCourseAsync(Course course) => _repository.AddAsync(course);
}