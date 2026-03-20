using API.Models;

namespace API.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
}