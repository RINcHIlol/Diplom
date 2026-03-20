using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.ModelsApi;

namespace diplom.Services;

public class RegService
{
    private readonly HttpClient _http;

    public RegService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<(bool Success, string? Error)> RegAsync(string login, string email, string password)
    {
        var response = await _http.PostAsJsonAsync(
            "api/reg/register",
            new { login, email, password });

        if (response.IsSuccessStatusCode)
            return (true, null);

        var errorObj = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        return (false, errorObj?.Message ?? "Ошибка регистрации");
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = "";
}
