using System;
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
                Question = dto.Question
            },

            (int)TaskType.SingleChoice => new SingleChoiceTaskViewModel
            {
                Id = dto.Id,
                Question = dto.Question,
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
                Answers = dto.Answers.Select(a => new SelectableAnswer
                {
                    Id = a.Id,
                    Text = a.AnswerText
                }).ToList()
            },

            _ => throw new Exception("Unknown task type")
        };
    }
}