using API.DTOs.Profile;
using API.Models;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories.Interfaces;

public interface IModulesRepository
{
    Task<List<ModuleProgressDto>> GetModulesForCourseAsync(int courseId, int? userId);
    Task<List<ModuleShortDTO>> GetMyModulesAsync(int courseId);
    Task<Module?> GetByIdAsync(int id);
    Task<int> GetMaxOrderIndex(int courseId);
    Task UpdateAsync(Module module);
    Task<Module> CreateAsync(Module module);
    Task<bool> IsOwnerAsync(int moduleId, int userId);
    Task DeleteAsync(int moduleId);
}