using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.ModelsApi;

namespace diplom.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(string baseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<(User?, string?)> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { username, password });
        if (!response.IsSuccessStatusCode)
            return (null, null);

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return (result?.User, result?.Token);
    }
    
    public async Task<User?> GetMeAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("api/auth/me");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<User>();
    }
}

public class LoginResponse
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = null!;
}