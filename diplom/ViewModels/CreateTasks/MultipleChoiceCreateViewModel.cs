using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using diplom.DTOs.Profile;
using diplom.ViewModels.Tasks;

namespace diplom.ViewModels.CreateTasks;

public class MultipleChoiceCreateViewModel : TaskCreateViewModel
{
    public ObservableCollection<AnswerOption> Answers { get; set; } = new();
    
    public override CreateTaskDto BuildDto()
    {
        return new CreateTaskDto
        {
            Question = Question,
            TaskTypeId = 3, 
            Answers = Answers.Select(a => new CreateAnswerDto()
            {
                AnswerText = a.Text,
                IsCorrect = a.IsSelected
            }).ToList()
        };
    }
    
    public ICommand AddAnswerCommand => new RelayCommand(() =>
        Answers.Add(new AnswerOption()));
}