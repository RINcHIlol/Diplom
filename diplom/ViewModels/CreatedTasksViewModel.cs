using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;
using diplom.ViewModels.Tasks;

namespace diplom.ViewModels;

public class CreatedTasksViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly NavigationService _navigation;
    private readonly TaskService _taskService;
    private readonly LessonsService _lessonService;
    private readonly MessageService _messageService;

    public ObservableCollection<TaskViewModel> Tasks { get; } = new();

    private int _currentIndex = 0;
    private bool _isLoading;
    private readonly SemaphoreSlim _loadLock = new(1, 1);
    
    public TaskViewModel? CurrentTask
    {
        get => _currentIndex >= 0 && _currentIndex < Tasks.Count ? Tasks[_currentIndex] : null;
        set
        {
            if (value == null) return;
            _currentIndex = Tasks.IndexOf(value);
            ResultText = null;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentTask));
        }
    }

    private string? _lessonName;
    public string? LessonName
    {
        get => _lessonName;
        set => SetProperty(ref _lessonName, value);
    }

    private string? _resultText;
    public string? ResultText
    {
        get => _resultText;
        set => SetProperty(ref _resultText, value);
    }

    public ICommand GoBackCommand { get; }
    public ICommand CreateCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteLessonCommand { get; }
    
    public CreatedTasksViewModel(MainWindowViewModel main, SessionService session, NavigationService navigation, TaskService taskService, LessonsService lessonService, MessageService messageService)
    {
        _main = main;
        _session = session;
        _navigation = navigation;
        _taskService = taskService;
        _lessonService = lessonService;
        _messageService = messageService;

        GoBackCommand = new RelayCommand(() => _main.ShowCreatedLessons());
        CreateCommand = new RelayCommand( () => _main.ShowCreateTask());
        DeleteLessonCommand = new RelayCommand(async () =>
        {
            var success = await _lessonService.DeleteLessonAsync(_navigation.CurrentLessonId!.Value);

            if (success)
            {
                _messageService.SetMessage("LessonsUpdated");
                _main.ShowCreatedLessons();
            }
        });
        EditCommand = new RelayCommand(() =>
        {
            if (CurrentTask == null) return;

            _navigation.CurrentTaskId = CurrentTask.Id;
            _main.ShowCreateTask();
        });

        LoadTasksAsync();
        _ = OnAppearingAsync();
    }
    
    private async Task LoadTasksAsync()
    {
        if (_navigation.CurrentLessonId == null)
            return;

        await _loadLock.WaitAsync();

        try
        {
            var lessonId = _navigation.CurrentLessonId.Value;

            var taskVMs = await _taskService.GetTasksAsync(lessonId);

            var orderedTasks = taskVMs
                .OrderBy(x => x.OrderIndex)
                .ToList();

            Tasks.Clear();
            
            for (int i = 0; i < orderedTasks.Count; i++)
            {
                var vm = orderedTasks[i];

                vm.IsCorrect = null;
                vm.DisplayIndex = i + 1;

                Tasks.Add(vm);
            }

            _currentIndex = 0;
            OnPropertyChanged(nameof(CurrentTask));
        }
        finally
        {
            _loadLock.Release();
        }
    }
    
    public async Task OnAppearingAsync()
    {
        var msg = _messageService.GetMessage();

        if (msg == "TasksUpdated")
        {
            _messageService.SetMessage(null); 
            await LoadTasksAsync();
        }
    }
}