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

    public async Task<List<CourseProgressDto>> GetCoursesAsync(int? userId)
    {
        return await _repository.GetCoursesForUserAsync(userId);
    }
    
    public async Task<List<CourseShortDto>> GetCoursesForCreatorAsync(int userId)
    {
        return await _repository.GetMyCoursesAsync(userId);
    }

    public async Task<CourseShortDto> GetCourseAsync(int courseId)
    {
        var course = await _repository.GetByIdAsync(courseId);
        
        var courseDto = new CourseShortDto()
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Image = course.Image,
            CreatedAt = course.CreatedAt,
        };
        return courseDto;
    }
    
    public async Task<Course> CreateCourseAsync(int userId, CreateUpdateCourseDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Image = dto.Image,
            CreatedAt = DateTime.UtcNow,
            CreatorUserId = userId
        };

        return await _repository.CreateAsync(course);
    }

    public async Task<bool> UpdateCourseAsync(int userId, int courseId, CreateUpdateCourseDto dto)
    {
        var course = await _repository.GetByIdAsync(courseId);

        if (course == null)
            return false;

        if (course.CreatorUserId != userId)
            return false;

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.Image = dto.Image;

        await _repository.UpdateAsync(course);

        return true;
    }
    
    public async Task<bool> IsOwnerAsync(int courseId, int userId)
    {
        return await _repository.IsOwnerAsync(courseId, userId);
    }
    
    public async Task<bool> DeleteAsync(int courseId)
    {
        var course = await _repository.GetByIdAsync(courseId);
        
        if (course == null)
            return false;
        
        await _repository.DeleteAsync(courseId);
        return true;
    }
}