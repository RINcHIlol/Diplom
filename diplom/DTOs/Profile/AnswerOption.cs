// using diplom.ViewModels;
// using diplom.ViewModels.CreateTasks;
//
// public class AnswerOption : ViewModelBase
// {
//     private string _text;
//     public string Text
//     {
//         get => _text;
//         set => SetProperty(ref _text, value);
//     }
//
//     private bool _isSelected;
//     public bool IsSelected
//     {
//         get => _isSelected;
//         set
//         {
//             if (SetProperty(ref _isSelected, value) && value)
//                 _parent?.Select(this);
//         }
//     }
//
//     public bool IsCorrect { get; set; }
//
//     private SingleChoiceCreateViewModel? _parent;
//     public void SetParent(SingleChoiceCreateViewModel parent)
//         => _parent = parent;
// }

using System;
using diplom.ViewModels;

public class AnswerOption : ViewModelBase
{
    private string _text;
    public string Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetProperty(ref _isSelected, value) && value)
                _onSelected?.Invoke(this);
        }
    }

    public bool IsCorrect { get; set; }

    private Action<AnswerOption>? _onSelected;

    public void SetOnSelected(Action<AnswerOption> handler)
        => _onSelected = handler;
    
    public Action<AnswerOption>? OnRemove { get; set; }

    public void Remove()
    {
        OnRemove?.Invoke(this);
    }
}