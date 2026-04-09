using System.Windows.Input;

namespace diplom.ViewModels.Tasks;

public abstract class TaskViewModel : ViewModelBase
{
    public int Id { get; set; }
    public string Question { get; set; }

    private bool? _isCorrect;
    public bool? IsCorrect
    {
        get => _isCorrect;
        set => SetProperty(ref _isCorrect, value);
    }

    public ICommand SubmitCommand => new RelayCommand(Submit);

    public abstract void Submit();
}