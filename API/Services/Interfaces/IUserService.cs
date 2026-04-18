using API.Models;
using Task = System.Threading.Tasks.Task;

namespace API.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserAsync(int id);
    Task<User> GetUserByLoginAsync(string login);
    Task<User> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    Task<bool> UpdateUserAsync(int userId, RegRequest user);
    Task<bool> UpdateXpAsync(int xp, int userId);
}