using API.Models;
using API.Repositories;

namespace API.Services;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserAsync(int id);
    Task<User> GetUserByLoginAsync(string login);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository) => _repository = repository;

    public Task<List<User>> GetAllUsersAsync() => _repository.GetAllAsync();
    public Task<User> GetUserAsync(int id) => _repository.GetByIdAsync(id);
    public Task<User> GetUserByLoginAsync(string login) => _repository.GetByLoginAsync(login);
}