using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories;

public class ModulesRepository : IModulesRepository
{
    private readonly AppDbContext _context;
    public ModulesRepository(AppDbContext context) => _context = context;

    public async Task<List<ModuleProgressDto>> GetModulesForCourseAsync(int courseId, int? userId)
    {
        var modules = await _context.Modules
            .Where(m => m.CourseId == courseId)
            .Include(m => m.Lessons)
            .ToListAsync();
        
        if (userId == null)
        {
            return modules.Select(m => new ModuleProgressDto
            {
                ModuleId = m.Id,
                Title = m.Title,
                OrderIndex = m.OrderIndex,
                TotalLessons = m.Lessons.Count,
                CompletedLessons = 0,
                ProgressPercent = 0
            }).ToList();
        }

        var userProgresses = await _context.UserProgresses
            .Where(p => p.UserId == userId && p.IsCompleted)
            .Select(p => p.LessonId)
            .ToListAsync();

        return modules.Select(m =>
        {
            var lessonIds = m.Lessons.Select(l => l.Id).ToList();
            var totalLessons = lessonIds.Count;
            var completedLessons = lessonIds.Count(id => userProgresses.Contains(id));

            return new ModuleProgressDto
            {
                ModuleId = m.Id,
                Title = m.Title,
                OrderIndex = m.OrderIndex,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                ProgressPercent = totalLessons == 0 ? 0 : (double)completedLessons / totalLessons * 100
            };
        }).ToList();
    }
}