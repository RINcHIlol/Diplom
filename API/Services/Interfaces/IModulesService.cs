using API.DTOs.Profile;
using API.Models;

namespace API.Services.Interfaces;

public interface IModulesService
{
    Task<List<ModuleProgressDto>> GetModulesAsync(int courseId, int? userId);
    Task<List<ModuleShortDTO>> GetModulesForCreatorAsync(int courseId);
    Task<ModuleShortDTO> GetModuleAsync(int moduleId);
    Task<Module> CreateModuleAsync(CreateUpdateModuleDto dto, int courseId);
    Task<bool> UpdateModuleAsync(int moduleId, CreateUpdateModuleDto dto);
    Task<bool> IsOwnerAsync(int moduleId, int userId);
    Task<bool> DeleteAsync(int moduleId);
}