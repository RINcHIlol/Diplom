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
    
    public async Task<List<LessonProgressDto>> GetLessonsAsync(int moduleId)
    {
        try
        {
            var lessons = await _http.GetFromJsonAsync<List<LessonProgressDto>>($"api/lessons/{moduleId}");
            return lessons ?? new List<LessonProgressDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки модулей: {ex.Message}");
            return new List<LessonProgressDto>();
        }
    }
}