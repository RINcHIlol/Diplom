using System.Collections.Generic;
using System.Linq;

namespace diplom.ViewModels.Tasks;

public class MultipleChoiceTaskViewModel : TaskViewModel
{
    public List<SelectableAnswer> Answers { get; set; } = new();
    public override List<int> GetAnswerIds()
    {
        return Answers
            .Where(a => a.IsSelected)
            .Select(a => a.Id)
            .ToList();
    }
    
    public override void Submit()
    {
        var selected = Answers
            .Where(a => a.IsSelected)
            .Select(a => a.Id)
            .ToList();

        // 👉 отправка списка selected
    }
}

public class SelectableAnswer
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsSelected { get; set; }
}