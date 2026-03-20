using API.DTOs.Profile;

namespace API.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileResponse> GetProfileById(int id);
}