using diplom.ModelsApi;

namespace diplom.DTOs.Profile;

public class LoginResponse
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = null!;
}