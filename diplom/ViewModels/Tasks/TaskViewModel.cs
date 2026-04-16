using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;

namespace diplom.ViewModels.Tasks;

public abstract class TaskViewModel : ViewModelBase
{
    public int Id { get; set; }
    public string Question { get; set; }
    public TaskDto Dto { get; set; }

    private bool? _isCorrect;
    public bool? IsCorrect
    {
        get => _isCorrect;
        set
        {
            SetProperty(ref _isCorrect, value);
            OnPropertyChanged(nameof(IsLocked));
        }
    }

    public bool IsLocked => IsCorrect == true;

    public ICommand SubmitCommand => new RelayCommand(Submit);
    
    public virtual List<int> GetAnswerIds() => new();
    public virtual string? GetTextAnswer() => null;
    public virtual List<MatchDto> GetMatches() => new();

    public abstract void Submit();
}