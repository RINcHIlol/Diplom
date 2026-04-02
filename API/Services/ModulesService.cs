using API.Services.Interfaces;

namespace API.Services;

public class ModulesService : IModulesService
{
    private readonly IModulesService _repository;
    public ModulesService(IModulesService repository) => _repository = repository;
    
}