using API.Data;
using API.DTOs.Profile;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
}