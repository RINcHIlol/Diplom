using API.DTOs.Profile;

namespace API.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<TaskDto>> GetByLessonIdAsync(int lessonId);
    Task<bool> GetLessonCompletedAsync(int lessonId, int userId);
    Task<List<TaskProgressDto>> GetTaskProgress(int userId, int lessonId);
    Task<bool> Submit(SubmitDto dto, int userId);
}