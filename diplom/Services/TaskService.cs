using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using diplom.DTOs.Profile;
using diplom.ViewModels.Tasks;

namespace diplom.Services;

public class TaskService
{
    private readonly HttpClient _http;

    public TaskService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<TaskViewModel>> GetTasksAsync(int lessonId)
    {
        var dtos = await _http.GetFromJsonAsync<List<TaskDto>>($"api/tasks/lesson/{lessonId}");

        var tasks = dtos.Select(TaskFactory.Create).ToList();

        return tasks;
    }
    
    public async Task<List<TaskProgressDto>> GetTaskProgressAsync(int lessonId)
    {
        return await _http.GetFromJsonAsync<List<TaskProgressDto>>($"api/tasks/lesson/{lessonId}/progress");
    }

    public async Task<bool> SubmitAsync(int taskId, List<int> answerIds, string? textAnswer = null, List<MatchDto>? matches = null)
    {
        var dto = new SubmitDto
        {
            TaskId = taskId,
            AnswerIds = answerIds,
            TextAnswer = textAnswer,
            Matches = matches
        };

        var response = await _http.PostAsJsonAsync("api/tasks/submit", dto);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Server error: {content}");

        var result = System.Text.Json.JsonSerializer.Deserialize<SubmitResponseDto>(content,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result?.IsCorrect ?? false;
    }
    
    public async Task<bool> IsLessonCompletedAsync(int lessonId)
    {
        var response = await _http.GetFromJsonAsync<bool>($"api/tasks/lesson/{lessonId}/completed");
        return response;
    }
    
    public async Task CreateTaskAsync(CreateTaskDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/tasks/add", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
    
    
    //new
    public async Task UpdateTaskAsync(int taskId, CreateTaskDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/tasks/{taskId}", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
    
    public async Task<TaskDto> GetByIdTaskAsync(int taskId)
    {
        return await _http.GetFromJsonAsync<TaskDto>($"api/tasks/{taskId}");
    }
}

public class SubmitResponseDto
{
    public bool IsCorrect { get; set; }
}