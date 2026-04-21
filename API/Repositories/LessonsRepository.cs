using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories;

public class LessonsRepository : ILessonsRepository
{
    private readonly AppDbContext _context;
    public LessonsRepository(AppDbContext context) => _context = context;

    public async Task<List<LessonProgressDto>> GetLessonsForModuleAsync(int moduleId, int? userId)
    {
        var lessons = await _context.Lessons
            .Where(l => l.ModuleId == moduleId)
            .OrderBy(l => l.OrderIndex)
            .ToListAsync();

        if (userId == null)
        {
            return lessons.Select(l => new LessonProgressDto
            {
                LessonId = l.Id,
                Title = l.Title,
                OrderIndex = l.OrderIndex,
                IsCompleted = false
            }).ToList();
        }

        var completedLessonIds = await _context.UserProgresses
            .Where(p => p.UserId == userId && p.IsCompleted)
            .Select(p => p.LessonId)
            .ToListAsync();

        return lessons.Select(l => new LessonProgressDto
        {
            LessonId = l.Id,
            Title = l.Title,
            OrderIndex = l.OrderIndex,
            IsCompleted = completedLessonIds.Contains(l.Id)
        }).ToList();
    }
    
    public async Task<List<LessonShortDTO>> GetMyLessonsAsync(int moduleId)
    {
        return await _context.Lessons
            .Where(x => x.ModuleId == moduleId)
            .Select(c => new LessonShortDTO
            {
                Id = c.Id,
                Title = c.Title,
                OrderIndex = c.OrderIndex,
            })
            .ToListAsync();
    }
    
    public async Task<Lesson> CreateAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    public async Task<int> GetMaxOrderIndex(int moduleId)
    {
        return await _context.Lessons.Where(x => x.ModuleId == moduleId).MaxAsync(x => (int?)x.OrderIndex) ?? 0;
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsOwnerAsync(int lessonId, int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user.RoleId == 2)
        {
            return true;
        }
        return await _context.Lessons.Include(x => x.Module)
            .AnyAsync(m =>
                m.Id == lessonId &&
                m.Module.Course.CreatorUserId == userId
            );
    }

    public async Task DeleteAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FirstOrDefaultAsync(c => c.Id == lessonId);
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }
}