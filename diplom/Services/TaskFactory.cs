using System;
using System.Collections.ObjectModel;
using System.Linq;
using diplom.DTOs.Profile;
using diplom.Enums;
using diplom.ViewModels.Tasks;

namespace diplom.Services;

public static class TaskFactory
{
    public static TaskViewModel Create(TaskDto dto)
    {
        return dto.TaskTypeId switch
        {
            (int)TaskType.Text => new TextTaskViewModel
            {
                Id = dto.Id,
                Question = dto.Question,
                OrderIndex = dto.OrderIndex
            },

            (int)TaskType.SingleChoice => new SingleChoiceTaskViewModel
            {
                Id = dto.Id,
                Question = dto.Question,
                OrderIndex = dto.OrderIndex,
                Answers = dto.Answers.Select(a => new SelectableAnswer
                {
                    Id = a.Id,
                    Text = a.AnswerText
                }).ToList()
            },

            (int)TaskType.MultipleChoice => new MultipleChoiceTaskViewModel
            {
                Id = dto.Id,
                Question = dto.Question,
                OrderIndex = dto.OrderIndex,
                Answers = dto.Answers.Select(a => new SelectableAnswer
                {
                    Id = a.Id,
                    Text = a.AnswerText
                }).ToList()
            },
            
            (int)TaskType.Matching => CreateMatching(dto),
            
            (int)TaskType.Ordering => new OrderingTaskViewModel
            {
                Id = dto.Id,
                Question = dto.Question,
                OrderIndex = dto.OrderIndex,
                Items = new ObservableCollection<SelectableAnswer>(
                    dto.Answers
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(a => new SelectableAnswer
                        {
                            Id = a.Id,
                            Text = a.AnswerText
                        })
                )
            },
            
            (int)TaskType.Coding => new CodingTaskViewModel
            {
                Id = dto.Id,
                Question = dto.Question,
                OrderIndex = dto.OrderIndex,
            },
            
            (int)TaskType.TextQuestion => new TextQuestionTaskViewModel()
            {
                Id = dto.Id,
                OrderIndex = dto.OrderIndex,
                Question = dto.Question
            },
            

            // _ => throw new Exception("Unknown task type")
            _ => throw new Exception($"Unknown task type: {dto.TaskTypeId}")
        };
    }
    
    private static MatchingTaskViewModel CreateMatching(TaskDto dto)
    {
        var half = dto.Answers.Count / 2;

        var leftItems = dto.Answers.Take(half)
            .Select(a => new SelectableAnswer
            {
                Id = a.Id,
                Text = a.AnswerText
            })
            .ToList();

        var rightItems = dto.Answers.Skip(half)
            .Select(a => new SelectableAnswer
            {
                Id = a.Id,
                Text = a.AnswerText
            })
            .ToList();
        
        var rnd = new Random();
        var shuffledRight = rightItems
            .OrderBy(x => rnd.Next())
            .ToList();

        var pairs = leftItems.Select(l => new MatchingPair
        {
            Left = l,
            RightItems = shuffledRight
        }).ToList();

        return new MatchingTaskViewModel
        {
            Id = dto.Id,
            OrderIndex = dto.OrderIndex,
            Question = dto.Question,
            RightItems = shuffledRight,
            Pairs = pairs
        };
    }
}