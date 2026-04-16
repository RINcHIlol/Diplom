using API.DTOs.Profile;
using API.Models;
using Task = System.Threading.Tasks.Task;
using TaskEntity = API.Models.Task;

namespace API.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<List<TaskDto>> GetByLessonIdAsync(int lessonId);
    Task<bool> GetLessonCompletedAsync(int lessonId, int userId);
    Task<List<TaskProgressDto>> GetTaskProgress(int userId, int lessonId);
    Task<bool> Submit(SubmitDto dto, int userId);
    // Task<int> GetNextOrderIndex(int lessonId);
    // Task<TaskEntity> CreateAsync(CreateTaskDto dto);
    Task<TaskEntity> CreateAsync(TaskEntity task);
    Task AddAnswersAsync(List<TaskAnswer> answers);
    Task AddPairsAsync(List<MatchingPair> pairs);
    Task<int> GetNextOrderIndex(int lessonId);
    Task<List<TaskAnswer>> GetAnswersByTaskIdAsync(int taskId);
    Task UpdateAsync(int id, CreateTaskDto dto);
    Task<TaskDto?> GetByIdAsync(int id);
}