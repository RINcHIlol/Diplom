using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.DTOs.Profile;

namespace diplom.Services;

public class LessonsService
{
    private readonly HttpClient _http;

    public LessonsService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }
    
    // public async Task<List<LessonProgressDto>> GetLessonsAsync(int moduleId)
    // {
    //     try
    //     {
    //         var lessons = await _http.GetFromJsonAsync<List<LessonProgressDto>>($"api/lessons/{moduleId}");
    //         return lessons ?? new List<LessonProgressDto>();
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Ошибка загрузки модулей: {ex.Message}");
    //         return new List<LessonProgressDto>();
    //     }
    // }
    
    public async Task<List<LessonProgressDto>> GetLessonsAsync(int moduleId)
    {
        try
        {
            var lessons = await _http.GetFromJsonAsync<List<LessonProgressDto>>($"api/modules/{moduleId}/lessons");
            return lessons ?? new List<LessonProgressDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки модулей: {ex.Message}");
            return new List<LessonProgressDto>();
        }
    }
    
    public async Task<List<LessonShortDTO>> GetMyLessonsAsync(int moduleId)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<LessonShortDTO>>(
                $"api/modules/{moduleId}/lessons/my");

            return result ?? new List<LessonShortDTO>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetMyLessonsAsync error: {ex.Message}");
            return new List<LessonShortDTO>();
        }
    }

    public async Task<LessonShortDTO?> GetLessonAsync(int lessonId)
    {
        try
        {
            return await _http.GetFromJsonAsync<LessonShortDTO>(
                $"api/modules/{1}/lessons/{lessonId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetLessonAsync error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateLessonAsync(int moduleId, CreateUpdateLessonDTO dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(
                $"api/modules/{moduleId}/lessons", dto);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CreateLessonAsync error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateLessonAsync(int moduleId, int lessonId, CreateUpdateLessonDTO dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(
                $"api/modules/{moduleId}/lessons/{lessonId}", dto);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UpdateLessonAsync error: {ex.Message}");
            return false;
        }
    }
}