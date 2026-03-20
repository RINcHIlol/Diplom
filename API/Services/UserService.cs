using API.Models;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository) => _repository = repository;

    public Task<List<User>> GetAllUsersAsync() => _repository.GetAllAsync();
    public Task<User> GetUserAsync(int id) => _repository.GetByIdAsync(id);
    public Task<User> GetUserByLoginAsync(string login) => _repository.GetByLoginAsync(login);
    public Task<User> GetUserByEmailAsync(string email) => _repository.GetByEmailAsync(email);
    public Task AddUserAsync(User user) => _repository.AddAsync(user);
}