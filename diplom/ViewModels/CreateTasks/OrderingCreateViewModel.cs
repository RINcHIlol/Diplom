using System.Collections.ObjectModel;
using System.Linq;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;


public class OrderingCreateViewModel : TaskCreateViewModel
{
    public ObservableCollection<AnswerOption> Items { get; set; } = new();

    public OrderingCreateViewModel()
    {
        AddItem();
    }

    public void AddItem()
    {
        var item = new AnswerOption
        {
            OnRemove = RemoveItem
        };

        Items.Add(item);
    }

    public void RemoveItem(AnswerOption item)
    {
        if (Items.Count > 1)
            Items.Remove(item);
    }

    public void Add() => AddItem();

    public override CreateTaskDto BuildDto()
    {
        return new CreateTaskDto
        {
            TaskTypeId = 5,
            Question = Question,
            Answers = Items.Select((a, i) => new CreateAnswerDto
            {
                AnswerText = a.Text,
                OrderIndex = i
            }).ToList()
        };
    }
}