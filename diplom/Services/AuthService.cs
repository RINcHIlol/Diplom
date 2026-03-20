using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.ModelsApi;

namespace diplom.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<(User?, string?)> LoginAsync(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new
        {
            username,
            password
        });

        if (!response.IsSuccessStatusCode)
            return (null, null);

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return (result?.User, result?.Token);
    }

    public async Task<User?> GetMeAsync()
    {
        var response = await _http.GetAsync("api/auth/me");

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