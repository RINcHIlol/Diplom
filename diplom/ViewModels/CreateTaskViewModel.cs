using System.Collections.Generic;
using System.Windows.Input;
using diplom.DTOs.Profile;
using diplom.Services;
using diplom.ViewModels.CreateTasks;

namespace diplom.ViewModels;

public class CreateTaskViewModel : ViewModelBase
{
    private readonly TaskService _taskService;
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly NavigationService _navigationService;
    private readonly SessionService _session;

    public List<TaskTypeItem> TaskTypes { get; } = new()
    {
        new TaskTypeItem { Name = "TextQuestion", Id = 7 },
        new TaskTypeItem { Name = "SingleChoice", Id = 2 },
        new TaskTypeItem { Name = "MultipleChoice", Id = 3 },
        new TaskTypeItem { Name = "Ordering", Id = 5 },
        new TaskTypeItem { Name = "Matching", Id = 4 },
        new TaskTypeItem { Name = "Text", Id = 1 },
        // new TaskTypeItem { Name = "Coding", Id = 6 }
    };

    public TaskTypeItem? SelectedTask { get; set; }
    
    private TaskCreateViewModel? _current;
    public TaskCreateViewModel? Current
    {
        get => _current;
        set => SetProperty(ref _current, value);
    }

    public ICommand ChangeTypeCommand => new RelayCommand(ChangeType);
    public ICommand SaveCommand => new RelayCommand(Save);

    public CreateTaskViewModel(MainWindowViewModel mainWindowViewModel, SessionService session, NavigationService navigationService, TaskService taskService)
    {
        _taskService = taskService;
        _mainWindowViewModel = mainWindowViewModel;
        _navigationService = navigationService;
        _session = session;
    }

    private void ChangeType()
    {
        if (SelectedTask == null)
            return;

        Current = SelectedTask.Id switch
        {
            7 => new TextQuestionCreateViewModel(),
            2 => new SingleChoiceCreateViewModel(),
            3 => new MultipleChoiceCreateViewModel(),
            5 => new OrderingCreateViewModel(),
            4 => new MatchingCreateViewModel(),
            1 => new TextCreateViewModel(),
            _ => null
        };
    }

    private async void Save()
    {
        var dto = Current?.BuildDto();
        if (dto == null) return;

        // await _taskService.CreateTask(dto);
    }
}