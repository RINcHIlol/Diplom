using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.DTOs.Profile;

namespace diplom.Services;

public class ModulesService
{
    private readonly HttpClient _http;

    public ModulesService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }
    
    public async Task<List<ModuleProgressDto>> GetModulesAsync(int courseId)
    {
        try
        {
            var modules = await _http.GetFromJsonAsync<List<ModuleProgressDto>>($"api/modules/{courseId}");
            return modules ?? new List<ModuleProgressDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки модулей: {ex.Message}");
            return new List<ModuleProgressDto>();
        }
    }
}