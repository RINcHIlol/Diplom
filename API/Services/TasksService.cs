using API.DTOs.Profile;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

public class TasksService : ITasksService
{
    private readonly ITasksRepository _repository;
    public TasksService(ITasksRepository repository) => _repository = repository;

    public async Task<List<TaskDto>> GetTasksAsync(int lessonId)
    {
        return await _repository.GetByLessonIdAsync(lessonId);
    }

    public async Task<bool> GetLessonCompletedAsync(int lessonId, int userId)
    {
        return await _repository.GetLessonCompletedAsync(lessonId, userId);
    }

    public async Task<bool> Submit(SubmitDto dto, int userId)
    {
        return await _repository.Submit(dto, userId);
    }
}