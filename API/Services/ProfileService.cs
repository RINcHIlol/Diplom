using API.DTOs.Profile;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _repository;
    public ProfileService(IProfileRepository repository) => _repository = repository;
    
    public Task<ProfileResponse> GetProfileById(int id) => _repository.GetProfileById(id);
}
