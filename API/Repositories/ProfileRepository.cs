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
            ProgressPercent = 0 //доделать(пока впадлу)
        }).ToList();

        var profile = new ProfileResponse
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            Xp = user.Xp,
            Role = user.Role.Title,
            Courses = coursesDTO
        };

        return profile;
    }
}