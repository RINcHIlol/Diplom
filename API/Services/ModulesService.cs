using API.DTOs.Profile;
using API.Models;
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
    
    public async Task<List<ModuleShortDTO>> GetModulesForCreatorAsync(int courseId)
    {
        return await _repository.GetMyModulesAsync(courseId);
    }
    
    public async Task<ModuleShortDTO> GetModuleAsync(int moduleId)
    {
        var module = await _repository.GetByIdAsync(moduleId);
        
        var moduleDto = new ModuleShortDTO()
        {
            Id = module.Id,
            Title = module.Title,
            OrderIndex = module.OrderIndex,
        };
        return moduleDto;
    }
    
    public async Task<Module> CreateModuleAsync(CreateUpdateModuleDto dto, int courseId)
    {
        var orderIndex = await _repository.GetMaxOrderIndex(courseId);
        var module = new Module
        {
            Title = dto.Title,
            OrderIndex = orderIndex,
            CourseId = courseId,
        };

        return await _repository.CreateAsync(module);
    }

    public async Task<bool> UpdateModuleAsync(int moduleId, CreateUpdateModuleDto dto)
    {
        var module = await _repository.GetByIdAsync(moduleId);

        if (module == null)
            return false;

        module.Title = dto.Title;

        await _repository.UpdateAsync(module);

        return true;
    }

    public async Task<bool> IsOwnerAsync(int moduleId, int userId)
    {
        return await _repository.IsOwnerAsync(moduleId, userId);
    }
    
    public async Task<bool> DeleteAsync(int moduleId)
    {
        var module = await _repository.GetByIdAsync(moduleId);
        
        if (module == null)
            return false;
        
        await _repository.DeleteAsync(moduleId);
        return true;
    }
}