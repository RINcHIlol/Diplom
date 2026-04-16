using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public abstract class TaskCreateViewModel : ViewModelBase
{
    // public string Question { get; set; }
    private string _question;
    public string Question
    {
        get => _question;
        set => SetProperty(ref _question, value);
    }

    public abstract CreateTaskDto BuildDto();
    
    public virtual void LoadFromDto(TaskDto dto)
    {
        // по умолчанию ничего
    }
}