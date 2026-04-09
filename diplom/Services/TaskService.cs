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

    public async Task<bool> SubmitAsync(int taskId, List<int> answerIds, string? textAnswer = null)
    {
        var dto = new SubmitDto
        {
            TaskId = taskId,
            AnswerIds = answerIds,
            TextAnswer = textAnswer
        };

        var response = await _http.PostAsJsonAsync("api/tasks/submit", dto);
        var result = await response.Content.ReadFromJsonAsync<SubmitResponseDto>();

        return result?.IsCorrect ?? false;
    }
    
    public async Task<bool> IsLessonCompletedAsync(int lessonId)
    {
        var response = await _http.GetFromJsonAsync<bool>($"api/tasks/lesson/{lessonId}/completed");
        return response;
    }
}

public class SubmitResponseDto
{
    public bool IsCorrect { get; set; }
}