using API.DTOs.Profile;

namespace API.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<TaskDto>> GetByLessonIdAsync(int lessonId);
    Task<bool> GetLessonCompletedAsync(int lessonId, int userId);
    Task<bool> Submit(SubmitDto dto, int userId);
}