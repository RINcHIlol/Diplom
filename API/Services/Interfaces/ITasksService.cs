using API.DTOs.Profile;

using TaskEntity = API.Models.Task;

namespace API.Services.Interfaces;

public interface ITasksService
{
    Task<List<TaskDto>> GetTasksAsync(int lessonId);
    Task<bool> GetLessonCompletedAsync(int lessonId, int userId);
    Task<List<TaskProgressDto>> GetTaskProgressAsync(int userId, int lessonId);
    Task<bool> Submit(SubmitDto dto, int userId);
    Task<TaskDto> CreateAsync(CreateTaskDto dto);
    Task UpdateAsync(int id, CreateTaskDto dto);
    Task<TaskDto?> GetByIdAsync(int id);
    Task<bool> IsOwnerAsync(int taskId, int userId);
    Task<bool> DeleteAsync(int taskId);
}