using System.Collections.Generic;
using System.Linq;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public class TextQuestionCreateViewModel : TaskCreateViewModel
{
    private string _correctAnswer;
    public string CorrectAnswer
    {
        get => _correctAnswer;
        set => SetProperty(ref _correctAnswer, value);
    }

    private string _userAnswer;
    public string UserAnswer
    {
        get => _userAnswer;
        set => SetProperty(ref _userAnswer, value);
    }

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
    
    public override void LoadFromDto(TaskDto dto)
    {
        Question = dto.Question;
        CorrectAnswer = dto.Answers.FirstOrDefault()?.AnswerText;


        UserAnswer = string.Empty;

        OnPropertyChanged(nameof(UserAnswer));
    }
}