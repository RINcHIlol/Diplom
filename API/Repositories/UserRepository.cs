using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> GetByLoginAsync(string login);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<List<User>> GetAllAsync() => await _context.Users.ToListAsync();
    public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
    public async Task<User> GetByLoginAsync(string login) => await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
}