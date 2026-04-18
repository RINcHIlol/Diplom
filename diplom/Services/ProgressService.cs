using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.DTOs.Profile;

namespace diplom.Services;

public class ProgressService
{
    private readonly HttpClient _http;

    public ProgressService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }
    
    public async Task<bool> UpdateXpAsync(int xp)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(
                $"api/users/{xp}", xp);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UpdateXpAsync error: {ex.Message}");
            return false;
        }
    }
}