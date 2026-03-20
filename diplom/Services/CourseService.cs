using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.ModelsApi;

namespace diplom.Services;

public class CourseApiService
{
    private readonly HttpClient _http;

    public CourseApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<Course>> GetCoursesAsync()
    {
        try
        {
            var courses = await _http.GetFromJsonAsync<List<Course>>("api/courses");
            return courses ?? new List<Course>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки курсов: {ex.Message}");
            return new List<Course>();
        }
    }
}