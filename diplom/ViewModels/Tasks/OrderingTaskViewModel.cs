using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace diplom.ViewModels.Tasks;

public class OrderingTaskViewModel : TaskViewModel
{
    public ObservableCollection<SelectableAnswer> Items { get; set; } = new();

    public ObservableCollection<SelectableAnswer> SelectedItems { get; set; } = new();

    public SelectableAnswer? SelectedItem { get; set; }
    public override List<int> GetAnswerIds()
    {
        return SelectedItems.Select(i => i.Id).ToList();
    }

    public void AddToOrder(SelectableAnswer item)
    {
        if (SelectedItems.Contains(item))
            return;

        SelectedItems.Add(item);
        Items.Remove(item);
    }

    public void RemoveFromOrder(SelectableAnswer item)
    {
        if (!SelectedItems.Contains(item))
            return;

        SelectedItems.Remove(item);
        Items.Add(item);
    }
    
    public void SetOrder(List<int> orderedIds)
    {
        if (orderedIds == null || orderedIds.Count == 0)
            return;

        var ordered = orderedIds
            .Select(id => Items.FirstOrDefault(x => x.Id == id))
            .Where(x => x != null)
            .ToList();

        Items.Clear();

        foreach (var item in ordered)
            Items.Add(item!);
    }

    public override void Submit()
    {
        // порядок = SelectedItems
    }
}