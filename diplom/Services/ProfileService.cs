using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.DTOs.Profile;

namespace diplom.Services;

public class ProfileService
{
    private readonly HttpClient _http;

    public ProfileService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<ProfileResponse?> GetProfileAsync()
    {
        return await _http.GetFromJsonAsync<ProfileResponse>("api/profile/me");
    }
}