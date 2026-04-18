using API.Data;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<List<User>> GetAllAsync() => await _context.Users.ToListAsync();
    public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
    public async Task<User> GetByLoginAsync(string login) => await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    public async Task<User> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int userId, RegRequest user)
    {
        var existing = await _context.Users.FindAsync(userId);

        if (existing == null) return;

        existing.Login = user.Login;
        existing.Email = user.Email;
        existing.PasswordHash = user.Password;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateExp(int xp, int userId)
    {
        var existing = await _context.Users.FindAsync(userId);
        
        if (existing == null) return;

        existing.Xp += xp;
        await _context.SaveChangesAsync();
    }
}