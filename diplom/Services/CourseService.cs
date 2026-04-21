using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.DTOs.Profile;
using diplom.ModelsApi;

namespace diplom.Services;

public class CourseApiService
{
    private readonly HttpClient _http;

    public CourseApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<CourseProgressDto>> GetCoursesAsync()
    {
        try
        {
            var courses = await _http.GetFromJsonAsync<List<CourseProgressDto>>("api/courses");
            return courses ?? new List<CourseProgressDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки курсов: {ex.Message}");
            return new List<CourseProgressDto>();
        }
    }
    
    public async Task<List<CourseShortDto>> GetCoursesForCreatorAsync()
    {
        try
        {
            var courses = await _http.GetFromJsonAsync<List<CourseShortDto>>("api/courses/my");
            return courses ?? new List<CourseShortDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки курсов: {ex.Message}");
            return new List<CourseShortDto>();
        }
    }
    
    public async Task<List<CourseShortDto>> GetCoursesForAdminAsync()
    {
        try
        {
            var courses = await _http.GetFromJsonAsync<List<CourseShortDto>>("api/courses/admin");
            return courses ?? new List<CourseShortDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки курсов: {ex.Message}");
            return new List<CourseShortDto>();
        }
    }
    
    public async Task<CourseShortDto?> GetCourseByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<CourseShortDto>($"api/courses/{id}");
        }
        catch
        {
            return null;
        }
    }
    
    public async Task<CourseShortDto?> CreateCourseAsync(CreateUpdateCourseDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/courses", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<CourseShortDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка создания курса: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> UpdateCourseAsync(int courseId, CreateUpdateCourseDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/courses/{courseId}", dto);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обновления курса: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteCourseAsync(int courseId)
    {
        try
        {
            var response = await _http.DeleteAsync(
                $"api/courses/{courseId}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DeleteCourseAsync error: {ex.Message}");
            return false;
        }
    }
}