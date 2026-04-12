using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public class MatchingCreateViewModel : TaskCreateViewModel
{
    public ObservableCollection<AnswerOption> Left { get; } = new();
    public ObservableCollection<AnswerOption> Right { get; } = new();

    public MatchingCreateViewModel()
    {
        Left.Add(new AnswerOption());
        Right.Add(new AnswerOption());
    }

    public void AddLeft() => Left.Add(new AnswerOption());
    public void AddRight() => Right.Add(new AnswerOption());

    public void RemoveLeft(AnswerOption item)
    {
        if (Left.Count > 1)
            Left.Remove(item);
    }

    public void RemoveRight(AnswerOption item)
    {
        if (Right.Count > 1)
            Right.Remove(item);
    }

    public override CreateTaskDto BuildDto()
    {
        var answers = new List<CreateAnswerDto>();

        answers.AddRange(Left.Select(a => new CreateAnswerDto
        {
            AnswerText = a.Text
        }));

        answers.AddRange(Right.Select(a => new CreateAnswerDto
        {
            AnswerText = a.Text
        }));

        return new CreateTaskDto
        {
            TaskTypeId = 4,
            Question = Question,
            Answers = answers
        };
    }
}