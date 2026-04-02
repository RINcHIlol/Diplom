using API.DTOs.Profile;

namespace API.Services.Interfaces;

public interface IModulesService
{
    Task<List<ModuleProgressDto>> GetModulesAsync(int courseId, int? userId);
}