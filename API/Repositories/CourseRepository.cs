using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;
    public CourseRepository(AppDbContext context) => _context = context;

    // public async Task<List<Course>> GetAllAsync() => await _context.Courses.ToListAsync();
    
    public async Task<List<CourseProgressDto>> GetCoursesForUserAsync(int? userId)
    {
        var courses = await _context.Courses
            .Include(c => c.Modules)
            .ThenInclude(m => m.Lessons)
            .ToListAsync();

        //no auth
        if (userId == null)
            return courses.Select(c => new CourseProgressDto
            {
                CourseId = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                Image = c.Image,
                TotalLessons = c.Modules.SelectMany(m => m.Lessons).Count(),
                CompletedLessons = 0,
                ProgressPercent = 0
            }).ToList();

        var userProgresses = await _context.UserProgresses
            .Where(p => p.UserId == userId)
            .Include(p => p.Lesson)
            .ThenInclude(l => l.Module)
            .ThenInclude(m => m.Course)
            .ToListAsync();

        return courses.Select(c =>
        {
            var totalLessons = c.Modules.SelectMany(m => m.Lessons).Count();

            var completedLessons = userProgresses
                .Count(p => p.Lesson.Module.Course.Id == c.Id && p.IsCompleted);

            return new CourseProgressDto
            {
                CourseId = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                Image = c.Image,
                
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                ProgressPercent = totalLessons == 0 ? 0 : (double)completedLessons / totalLessons * 100
            };
        }).ToList();
    }
    
    public async Task<List<CourseShortDto>> GetMyCoursesAsync(int userId)
    {
        return await _context.Courses
            .Where(c => c.CreatorUserId == userId)
            .Select(c => new CourseShortDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Image = c.Image,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }
    
    public async Task<Course> CreateAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Course course)
    {
        var existing = await _context.Courses
            .FirstOrDefaultAsync(x => x.Id == course.Id);

        if (existing == null)
            return;

        existing.Title = course.Title;
        existing.Description = course.Description;
        existing.Image = course.Image;

        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> IsOwnerAsync(int courseId, int userId)
    {
        return await _context.Courses.AnyAsync(m => m.Id == courseId && m.CreatorUserId == userId);
    }
    
    public async Task DeleteAsync(int courseId)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }
}