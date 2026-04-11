using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using diplom.Services;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using TaskEntity = API.Models.Task;

namespace API.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly AppDbContext _context;
    private readonly CodeRunnerService _runner;

    public TasksRepository(AppDbContext context, CodeRunnerService runner)
    {
        _context = context;
        _runner = runner;
    }

    public async Task<List<TaskDto>> GetByLessonIdAsync(int lessonId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.LessonId == lessonId)
            .Include(t => t.Answers)
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
                    OrderIndex = a.OrderIndex
                }).ToList()
        }).ToList();
    }

    public async Task<bool> GetLessonCompletedAsync(int lessonId, int userId)
    {
        var userProgress = await _context.UserProgresses
            .FirstOrDefaultAsync(x => x.LessonId == lessonId && x.UserId == userId);

        return userProgress?.IsCompleted ?? false;
    }

    public async Task<List<TaskProgressDto>> GetTaskProgress(int userId, int lessonId)
    {
        return await _context.UserTaskProgress
            .Where(x => x.UserId == userId && x.Task.LessonId == lessonId)
            .Select(x => new TaskProgressDto
            {
                TaskId = x.TaskId,
                IsCorrect = x.IsCorrect
            })
            .ToListAsync();
    }

    public async Task<bool> Submit(SubmitDto dto, int userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Answers)
            .FirstOrDefaultAsync(t => t.Id == dto.TaskId);

        if (task == null)
            throw new Exception("Task not found");

        var progress = await _context.UserTaskProgress
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == dto.TaskId);

        if (progress != null && progress.IsCorrect == true)
            return true;

        bool isCorrect = task.TaskTypeId switch
        {
            1 => true,
            2 => HandleSingleChoice(task, dto),
            3 => HandleMultipleChoice(task, dto),
            4 => HandleMatching(task, dto),
            5 => HandleOrdering(task, dto),
            6 => await HandleCoding(task, dto),
            7 => HandleTextInput(task, dto),
            _ => false
        };

        if (progress == null)
        {
            _context.UserTaskProgress.Add(new UserTaskProgress
            {
                UserId = userId,
                TaskId = dto.TaskId,
                IsCorrect = isCorrect,
                AnsweredAt = DateTime.UtcNow
            });
        }
        else
        {
            if (progress.IsCorrect == true)
                return isCorrect;

            progress.IsCorrect = isCorrect;
            progress.AnsweredAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        await UpdateLessonProgress(userId, task.LessonId);

        return isCorrect;
    }

    private async Task UpdateLessonProgress(int userId, int lessonId)
    {
        var totalTasks = await _context.Tasks
            .CountAsync(t => t.LessonId == lessonId);

        var doneTasks = await _context.UserTaskProgress
            .CountAsync(x =>
                x.UserId == userId &&
                x.IsCorrect &&
                x.Task.LessonId == lessonId);

        if (totalTasks == 0 || doneTasks != totalTasks)
            return;

        var lesson = await _context.UserProgresses
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.LessonId == lessonId);

        if (lesson == null)
        {
            _context.UserProgresses.Add(new UserProgress
            {
                UserId = userId,
                LessonId = lessonId,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            });
        }
        else
        {
            lesson.IsCompleted = true;
        }

        await _context.SaveChangesAsync();
    }

    private bool HandleSingleChoice(TaskEntity task, SubmitDto dto)
    {
        var correct = task.Answers.FirstOrDefault(a => a.IsCorrect);

        if (correct == null || dto.AnswerIds.Count != 1)
            return false;

        return correct.Id == dto.AnswerIds.First();
    }

    private bool HandleMultipleChoice(TaskEntity task, SubmitDto dto)
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

    private bool HandleMatching(TaskEntity task, SubmitDto dto)
    {
        var correct = _context.MatchingPairs
            .Where(p => p.TaskId == task.Id)
            .Select(p => new MatchDto
            {
                LeftId = p.LeftAnswerId,
                RightId = p.RightAnswerId
            })
            .ToList();

        var user = dto.Matches ?? new();

        return correct.Count == user.Count &&
               correct.All(c => user.Any(u =>
                   u.LeftId == c.LeftId && u.RightId == c.RightId));
    }

    private bool HandleOrdering(TaskEntity task, SubmitDto dto)
    {
        var correct = task.Answers
            .OrderBy(a => a.OrderIndex)
            .Select(a => a.Id)
            .ToList();

        return correct.SequenceEqual(dto.AnswerIds);
    }
    
    private bool HandleTextInput(TaskEntity task, SubmitDto dto)
    {
        var correctAnswer = task.Answers
            .FirstOrDefault(a => a.IsCorrect);

        var user = dto.TextAnswer;

        if (correctAnswer == null || string.IsNullOrWhiteSpace(user))
            return false;

        return Normalize(correctAnswer.AnswerText) == Normalize(user);
    }

    private async Task<bool> HandleCoding(TaskEntity task, SubmitDto dto)
    {
        var output = await _runner.RunAsync(dto.TextAnswer);

        return Normalize(output) == Normalize(task.ExpectedOutput);
    }

    private string Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "";

        return input
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Trim();
    }
}