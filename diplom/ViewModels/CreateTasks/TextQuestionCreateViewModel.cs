using System.Collections.Generic;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public class TextQuestionCreateViewModel : TaskCreateViewModel
{
    public string CorrectAnswer { get; set; }

    public override CreateTaskDto BuildDto()
    {
        return new CreateTaskDto
        {
            TaskTypeId = 7,
            Question = Question,
            Answers = new List<CreateAnswerDto>
            {
                new CreateAnswerDto
                {
                    AnswerText = CorrectAnswer,
                    IsCorrect = true,
                    OrderIndex = 0
                }
            }
        };
    }
}