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

    // public async Task<List<TaskDto>> GetByLessonIdAsync(int lessonId)
    // {
    //     var tasks = await _context.Tasks
    //         .Where(t => t.LessonId == lessonId)
    //         .Include(t => t.Answers)
    //         .OrderBy(t => t.OrderIndex)
    //         .ToListAsync();
    //
    //     return tasks.Select(t => new TaskDto
    //     {
    //         Id = t.Id,
    //         TaskTypeId = t.TaskTypeId,
    //         Question = t.Question,
    //         Content = t.Content,
    //         OrderIndex = t.OrderIndex,
    //         Answers = t.Answers
    //             .OrderBy(a => a.OrderIndex)
    //             .Select(a => new TaskAnswerDto
    //             {
    //                 Id = a.Id,
    //                 AnswerText = a.AnswerText,
    //                 OrderIndex = a.OrderIndex
    //             }).ToList()
    //         
    //     }).ToList();
    // }
    public async Task<List<TaskDto>> GetByLessonIdAsync(int lessonId)
{
    var tasks = await _context.Tasks
        .Where(t => t.LessonId == lessonId)
        .ToListAsync();

    var taskIds = tasks.Select(t => t.Id).ToList();

    var answers = await _context.TaskAnswers
        .Where(a => taskIds.Contains(a.TaskId))
        .ToListAsync();

    var pairs = await _context.MatchingPairs
        .Where(p => taskIds.Contains(p.TaskId))
        .ToListAsync();

    var result = new List<TaskDto>();

    foreach (var task in tasks)
    {
        var taskAnswers = answers
            .Where(a => a.TaskId == task.Id)
            .ToList();

        if (task.TaskTypeId == 4) // matching
        {
            var taskPairs = pairs
                .Where(p => p.TaskId == task.Id)
                .ToList();

            var leftIds = taskPairs.Select(p => p.LeftAnswerId).Distinct().ToList();
            var rightIds = taskPairs.Select(p => p.RightAnswerId).Distinct().ToList();

            var left = taskAnswers
                .Where(a => leftIds.Contains(a.Id))
                .Select(a => new TaskAnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect
                })
                .ToList();

            var right = taskAnswers
                .Where(a => rightIds.Contains(a.Id))
                .Select(a => new TaskAnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect
                })
                .ToList();

            var dto = new TaskDto
            {
                Id = task.Id,
                TaskTypeId = task.TaskTypeId,
                Question = task.Question,
                Content = task.Content,
                OrderIndex = task.OrderIndex,

                Answers = taskAnswers.Select(a => new TaskAnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    OrderIndex = a.OrderIndex,
                    IsCorrect = a.IsCorrect
                }).ToList(),

                MatchingPairs = taskPairs.Select(p => new MatchDto
                {
                    LeftId = p.LeftAnswerId,
                    RightId = p.RightAnswerId
                }).ToList()
            };

            result.Add(dto);
        }
        else
        {
            // обычные задания
            var dto = new TaskDto
            {
                Id = task.Id,
                Question = task.Question,
                TaskTypeId = task.TaskTypeId,
                Answers = taskAnswers.Select(a => new TaskAnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            result.Add(dto);
        }
    }

    return result;
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
    
    public async Task<int> GetNextOrderIndex(int lessonId)
    {
        var max = await _context.Tasks
            .Where(t => t.LessonId == lessonId)
            .MaxAsync(t => (int?)t.OrderIndex);

        return (max ?? 0) + 1;
    }
    
    public async Task<TaskEntity> CreateAsync(TaskEntity task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<List<TaskAnswer>> GetAnswersByTaskIdAsync(int taskId)
    {
        return await _context.TaskAnswers
            .Where(a => a.TaskId == taskId)
            .OrderBy(a => a.OrderIndex)
            .ToListAsync();
    }

    public async Task AddAnswersAsync(List<TaskAnswer> answers)
    {
        await _context.TaskAnswers.AddRangeAsync(answers);
        await _context.SaveChangesAsync();
    }

    public async Task AddPairsAsync(List<MatchingPair> pairs)
    {
        await _context.MatchingPairs.AddRangeAsync(pairs);
        await _context.SaveChangesAsync();
    }
    
    //new
    public async Task UpdateAsync(int id, CreateTaskDto dto)
    {
        var task = await _context.Tasks
            .Include(t => t.Answers)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            throw new Exception("Task not found");

        task.Question = dto.Question;
        task.Content = dto.Content;

        // ❗ удаляем старые ответы
        _context.TaskAnswers.RemoveRange(task.Answers);

        await _context.SaveChangesAsync();

        // ❗ создаём заново (как в Create)
        var answers = dto.Answers.Select(a => new TaskAnswer
        {
            TaskId = id,
            AnswerText = a.AnswerText,
            IsCorrect = a.IsCorrect,
            OrderIndex = a.OrderIndex
        }).ToList();

        await _context.TaskAnswers.AddRangeAsync(answers);
        await _context.SaveChangesAsync();

        // ❗ пары — тоже удалить и создать заново
        var oldPairs = _context.MatchingPairs.Where(p => p.TaskId == id);
        _context.MatchingPairs.RemoveRange(oldPairs);

        if (dto.TaskTypeId == 4)
        {
            var ordered = answers
                .OrderBy(a => a.OrderIndex)
                .ToList();

            int half = ordered.Count / 2;

            var pairs = new List<MatchingPair>();

            for (int i = 0; i < half; i++)
            {
                pairs.Add(new MatchingPair
                {
                    TaskId = id,
                    LeftAnswerId = ordered[i].Id,
                    RightAnswerId = ordered[i + half].Id
                });
            }

            await _context.MatchingPairs.AddRangeAsync(pairs);
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return null;

        var answers = await _context.TaskAnswers
            .Where(a => a.TaskId == id)
            .ToListAsync();

        var pairs = await _context.MatchingPairs
            .Where(p => p.TaskId == id)
            .ToListAsync();

        return new TaskDto
        {
            Id = task.Id,
            TaskTypeId = task.TaskTypeId,
            Question = task.Question,
            Content = task.Content,
            OrderIndex = task.OrderIndex,

            Answers = answers
                .OrderBy(a => a.OrderIndex)
                .Select(a => new TaskAnswerDto
                {
                    Id = a.Id,
                    AnswerText = a.AnswerText,
                    OrderIndex = a.OrderIndex,
                    IsCorrect = a.IsCorrect
                }).ToList(),

            MatchingPairs = pairs.Select(p => new MatchDto
            {
                LeftId = p.LeftAnswerId,
                RightId = p.RightAnswerId
            }).ToList()
        };
    }
}