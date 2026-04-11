using System.Collections.Generic;
using System.Linq;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.Tasks;

public class SingleChoiceTaskViewModel : TaskViewModel
{
    public List<SelectableAnswer> Answers { get; set; } = new();

    public SelectableAnswer? SelectedAnswer { get; set; }
    public override List<int> GetAnswerIds()
    {
        var selected = Answers.FirstOrDefault(a => a.IsSelected);
        return selected != null ? new List<int> { selected.Id } : new List<int>();
    }
    
    public override void Submit()
    {
        var selected = Answers.FirstOrDefault(a => a.IsSelected);

        if (selected != null)
        {
            var selectedId = selected.Id;

            // 👉 отправка selectedId на сервер
        }
    }
}