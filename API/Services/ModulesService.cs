using API.DTOs.Profile;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

public class ModulesService : IModulesService
{
    private readonly IModulesRepository _repository;
    public ModulesService(IModulesRepository repository) => _repository = repository;

    public async Task<List<ModuleProgressDto>> GetModulesAsync(int courseId, int? userId)
    {
        return await _repository.GetModulesForCourseAsync(courseId, userId);
    }
}