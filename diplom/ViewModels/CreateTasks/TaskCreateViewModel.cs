using diplom.DTOs.Profile;

namespace diplom.ViewModels.CreateTasks;

public abstract class TaskCreateViewModel : ViewModelBase
{
    public string Question { get; set; }

    public abstract CreateTaskDto BuildDto();
}