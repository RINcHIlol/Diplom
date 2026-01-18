using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
    // Task<Course> GetByIdAsync(int id);
    // Task AddAsync(Course course);
}

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;
    public CourseRepository(AppDbContext context) => _context = context;

    public async Task<List<Course>> GetAllAsync() => await _context.Courses.ToListAsync();
    // public async Task<Course> GetByIdAsync(int id) => await _context.Courses.FindAsync(id);
    // public async Task AddAsync(Course course)
    // {
    //     _context.Courses.Add(course);
    //     await _context.SaveChangesAsync();
    // }
}