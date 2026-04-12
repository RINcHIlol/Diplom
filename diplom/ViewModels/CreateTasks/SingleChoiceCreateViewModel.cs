using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using diplom.DTOs.Profile;
using diplom.ViewModels.CreateTasks;

namespace diplom.ViewModels.CreateTasks;
public class SingleChoiceCreateViewModel : TaskCreateViewModel
{
    public ObservableCollection<AnswerOption> Answers { get; set; } = new();

    public SingleChoiceCreateViewModel()
    {
        Answers.CollectionChanged += (_, __) =>
        {
            foreach (var a in Answers)
                a.SetOnSelected(Select);
        };
    }
    
    public override CreateTaskDto BuildDto()
    {
        return new CreateTaskDto
        {
            TaskTypeId = 2,
            Question = Question,
            Answers = Answers.Select((a, i) => new CreateAnswerDto
            {
                AnswerText = a.Text,
                IsCorrect = a.IsCorrect,
                OrderIndex = i
            }).ToList()
        };
    }

    public ICommand AddAnswerCommand => new RelayCommand(() =>
        Answers.Add(new AnswerOption()));

    public void Select(AnswerOption selected)
    {
        foreach (var a in Answers)
        {
            if (a != selected)
                a.IsSelected = false;

            a.IsCorrect = false;
        }

        selected.IsCorrect = true;
    }
}