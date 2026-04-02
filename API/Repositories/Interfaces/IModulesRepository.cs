using API.DTOs.Profile;

namespace API.Repositories.Interfaces;

public interface IModulesRepository
{
    Task<List<ModuleProgressDto>> GetModulesForCourseAsync(int courseId, int? userId);
}