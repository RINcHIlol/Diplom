using API.DTOs.Profile;

namespace API.Services.Interfaces;

public interface ITasksService
{
    Task<List<TaskDto>> GetTasksAsync(int lessonId);
    Task<bool> GetLessonCompletedAsync(int lessonId, int userId);
    Task<bool> Submit(SubmitDto dto, int userId);
}