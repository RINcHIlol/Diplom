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
    
    public async Task<List<ModuleShortDTO>> GetMyModulesAsync(int courseId)
    {
        return await _context.Modules
            .Where(x => x.CourseId == courseId)
            .Select(c => new ModuleShortDTO
            {
                Id = c.Id,
                Title = c.Title,
                OrderIndex = c.OrderIndex,
            })
            .ToListAsync();
    }
    
    public async Task<Module> CreateAsync(Module module)
    {
        _context.Modules.Add(module);
        await _context.SaveChangesAsync();
        return module;
    }

    public async Task<int> GetMaxOrderIndex(int courseId)
    {
        return await _context.Modules
            .Where(x => x.CourseId == courseId)
            .MaxAsync(x => (int?)x.OrderIndex) ?? 0;
    }

    public async Task<Module?> GetByIdAsync(int id)
    {
        return await _context.Modules.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Module module)
    {
        _context.Modules.Update(module);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsOwnerAsync(int moduleId, int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user.RoleId == 2)
        {
            return true;
        }
        return await _context.Modules
            .AnyAsync(m =>
                m.Id == moduleId &&
                m.Course.CreatorUserId == userId
            );
    }
    
    public async Task DeleteAsync(int moduleId)
    {
        var module = await _context.Modules.FirstOrDefaultAsync(c => c.Id == moduleId);
        _context.Modules.Remove(module);
        await _context.SaveChangesAsync();
    }
}