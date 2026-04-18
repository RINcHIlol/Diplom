using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using Task = System.Threading.Tasks.Task;
using TaskEntity = API.Models.Task;

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

    public async Task<List<TaskProgressDto>> GetTaskProgressAsync(int userId, int lessonId)
    {
        return await _repository.GetTaskProgress(userId, lessonId);
    }

    public async Task<bool> Submit(SubmitDto dto, int userId)
    {
        return await _repository.Submit(dto, userId);
    }
    
    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        var orderIndex = await _repository.GetNextOrderIndex(dto.LessonId);

        var task = new TaskEntity
        {
            TaskTypeId = dto.TaskTypeId,
            Question = dto.Question,
            Content = dto.Content,
            ExpectedOutput = dto.ExpectedOutput,
            LessonId = dto.LessonId,
            OrderIndex = orderIndex
        };

        var created = await _repository.CreateAsync(task);

        var answers = dto.Answers.Select(a => new TaskAnswer
        {
            TaskId = created.Id,
            AnswerText = a.AnswerText,
            IsCorrect = a.IsCorrect,
            OrderIndex = a.OrderIndex
        }).ToList();

        await _repository.AddAnswersAsync(answers);
        
        Console.WriteLine($"Pairs count: {dto.MatchingPairs?.Count}");

        if (dto.TaskTypeId == 4)
        {
            var orderedAnswers = answers
                .OrderBy(a => a.OrderIndex)
                .ToList();

            int half = orderedAnswers.Count / 2;

            var pairs = new List<MatchingPair>();

            for (int i = 0; i < half; i++)
            {
                pairs.Add(new MatchingPair
                {
                    TaskId = created.Id,
                    LeftAnswerId = orderedAnswers[i].Id,
                    RightAnswerId = orderedAnswers[i + half].Id
                });
            }

            await _repository.AddPairsAsync(pairs);
        }

        return new TaskDto
        {
            Id = created.Id,
            TaskTypeId = created.TaskTypeId,
            Question = created.Question,
            Content = created.Content,
            OrderIndex = created.OrderIndex
        };
    }
    
    public async Task UpdateAsync(int id, CreateTaskDto dto)
    {
        await _repository.UpdateAsync(id, dto);
    }
    
    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsOwnerAsync(int taskId, int userId)
    {
        return await _repository.IsOwnerAsync(taskId, userId);
    }
    
    public async Task<bool> DeleteAsync(int taskId)
    {
        var task = await _repository.GetByIdAsync(taskId);
        
        if (task == null)
            return false;
        
        await _repository.DeleteAsync(taskId);
        return true;
    }
}