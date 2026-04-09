using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

using Task = API.Models.Task;

namespace API.Repositories;


public class TasksRepository : ITasksRepository
{
    private readonly AppDbContext _context;

    public TasksRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> GetByLessonIdAsync(int lessonId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.LessonId == lessonId).Include( t=> t.Answers)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync();

        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            TaskTypeId = t.TaskTypeId,
            Question = t.Question,
            Content = t.Content,
            OrderIndex = t.OrderIndex,

            Answers = t.Answers
                .OrderBy(a => a.OrderIndex)
                .Select(a => new TaskAnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    OrderIndex = a.OrderIndex,
                    MatchKey = a.MatchKey
                }).ToList()
        }).ToList();
    }

    public async Task<bool> GetLessonCompletedAsync(int lessonId, int userId)
    {
        var userProgress = await _context.UserProgresses.FirstOrDefaultAsync(x => x.LessonId == lessonId && x.UserId == userId);
        if (userProgress == null)
        {
            throw new Exception("Task not found");
        }

        return userProgress.IsCompleted;
    }
    
    public async Task<bool> Submit(SubmitDto dto, int userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Answers)
            .FirstOrDefaultAsync(t => t.Id == dto.TaskId);

        if (task == null)
            throw new Exception("Task not found");

        bool isCorrect = task.TaskTypeId switch
        {
            1 => await HandleTextTask(task, userId),
            2 => HandleSingleChoice(task, dto),
            3 => HandleMultipleChoice(task, dto),
            _ => throw new Exception("Unknown task type")
        };

        if (isCorrect)
        {
            await MarkLessonCompleted(task.LessonId, userId);
        }

        return isCorrect;
    }
    
    private async Task<bool> HandleTextTask(Task task, int userId)
    {
        // всегда true, можно добавить проверку текста если нужно
        return true;
    }

    private bool HandleSingleChoice(Task task, SubmitDto dto)
    {
        var correct = task.Answers.FirstOrDefault(a => a.IsCorrect);

        if (correct == null || dto.AnswerIds.Count != 1)
            return false;

        return correct.Id == dto.AnswerIds.First();
    }

    private bool HandleMultipleChoice(Task task, SubmitDto dto)
    {
        var correctIds = task.Answers
            .Where(a => a.IsCorrect)
            .Select(a => a.Id)
            .OrderBy(x => x)
            .ToList();

        var userIds = dto.AnswerIds
            .OrderBy(x => x)
            .ToList();

        return correctIds.SequenceEqual(userIds);
    }

    private async System.Threading.Tasks.Task MarkLessonCompleted(int lessonId, int userId)
    {
        var exists = await _context.UserProgresses
            .AnyAsync(p => p.UserId == userId && p.LessonId == lessonId);

        if (exists)
            return;

        _context.UserProgresses.Add(new UserProgress
        {
            UserId = userId,
            LessonId = lessonId,
            IsCompleted = true,
            CompletedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
}