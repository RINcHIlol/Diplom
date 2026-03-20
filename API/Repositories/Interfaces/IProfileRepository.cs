using API.DTOs.Profile;
using API.Models;

namespace API.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<ProfileResponse> GetProfileById(int id);
}