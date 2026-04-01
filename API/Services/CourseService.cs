using API.DTOs.Profile;
using API.Models;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    public CourseService(ICourseRepository repository) => _repository = repository;

    // public Task<List<Course>> GetAllCoursesAsync() => _repository.GetAllAsync();
    public async Task<List<CourseProgressDto>> GetCoursesAsync(int? userId)
    {
        return await _repository.GetCoursesForUserAsync(userId);
    }
}