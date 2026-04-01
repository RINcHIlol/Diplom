using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            var completedLessons = userProgresses.Count(p => p.Lesson.Module.Course.Id == c.Id);

            return new CourseProgressDto
            {
                CourseId = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                Image = c.Image,
                ProgressPercent = totalLessons == 0 ? 0 : (double)completedLessons / totalLessons * 100
            };
        }).ToList();
    }
}