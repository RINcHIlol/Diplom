using System;
using diplom.ViewModels;

public class AnswerOption : ViewModelBase
{
    public int TempId { get; set; } = Guid.NewGuid().GetHashCode();

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