using API.Models;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> GetByLoginAsync(string login);
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(int userId, RegRequest user);
    Task UpdateExp(int xp, int userId);
}