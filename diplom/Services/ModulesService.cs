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
            var result = await _http.GetFromJsonAsync<List<ModuleProgressDto>>(
                $"api/courses/{courseId}/modules");

            return result ?? new List<ModuleProgressDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetModulesAsync error: {ex.Message}");
            return new List<ModuleProgressDto>();
        }
    }

    public async Task<List<ModuleShortDTO>> GetMyModulesAsync(int courseId)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<ModuleShortDTO>>(
                $"api/courses/{courseId}/modules/my");

            return result ?? new List<ModuleShortDTO>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetMyModulesAsync error: {ex.Message}");
            return new List<ModuleShortDTO>();
        }
    }

    public async Task<ModuleShortDTO?> GetModuleAsync(int moduleId)
    {
        try
        {
            return await _http.GetFromJsonAsync<ModuleShortDTO>(
                $"api/courses/{1}/modules/{moduleId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetModuleAsync error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateModuleAsync(int courseId, CreateUpdateModuleDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
                $"api/courses/{courseId}/modules", dto);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CreateModuleAsync error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateModuleAsync(int courseId, int moduleId, CreateUpdateModuleDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(
                $"api/courses/{courseId}/modules/{moduleId}", dto);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UpdateModuleAsync error: {ex.Message}");
            return false;
        }
    }
}