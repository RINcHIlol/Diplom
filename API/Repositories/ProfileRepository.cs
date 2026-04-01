using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;
    public ProfileRepository(AppDbContext context) => _context = context;

    public async Task<ProfileResponse> GetProfileById(int id)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Progresses)
            .ThenInclude(p => p.Lesson)
            .ThenInclude(l => l.Module)
            .ThenInclude(m => m.Course)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        var courses = user.Progresses
            .Select(p => p.Lesson.Module.Course)
            .Distinct()
            .ToList();

        var coursesDTO = courses.Select(course => new CourseProgressDto
        {
            CourseId = course.Id,
            Title = course.Title,
            Description = course.Description,
            CreatedAt = course.CreatedAt,
            ProgressPercent = user.Progresses
                                  .Where(p => p.Lesson.Module.Course.Id == course.Id)
                                  .Count() 
                              / (double)_context.Lessons
                                  .Include(l => l.Module)
                                  .ThenInclude(m => m.Course)
                                  .Count(l => l.Module.Course.Id == course.Id)
                              * 100
        }).ToList();
        
        
        var lvls = await _context.Lvls
            .OrderBy(l => l.Xp)
            .ToListAsync();
        var currentLvl = lvls.LastOrDefault(l => user.Xp >= l.Xp);
        var nextLvl = lvls.FirstOrDefault(l => l.Xp > user.Xp);
        int currentXp = currentLvl?.Xp ?? 0;
        int nextXp = nextLvl?.Xp ?? currentXp;
        double progress = nextLvl == null
            ? 1.0
            : (double)(user.Xp - currentXp) / (nextXp - currentXp);
        
        
        var profile = new ProfileResponse
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            Xp = user.Xp,
            Role = user.Role.Title,
            
            CurrentLvl = currentLvl?.Id ?? 1,
            CurrentLvlTitle = currentLvl?.Title ?? "Рекрут",
            Progress = progress,
            NextLvlXp = nextXp,
            
            Courses = coursesDTO
        };

        return profile;
    }
}