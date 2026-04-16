using System.Collections.ObjectModel;
using System.Linq;
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
    }
    
    private async void LoadTasksAsync()
    {
        if (_navigation.CurrentLessonId == null)
            return;
        
        Tasks.Clear();

        var lesson = _navigation.CurrentLessonId;

        var taskVMs = await _taskService.GetTasksAsync(lesson.Value);
        var progress = await _taskService.GetTaskProgressAsync(lesson.Value);

        var progressMap = progress.ToDictionary(x => x.TaskId);

        foreach (var vm in taskVMs)
        {
            Tasks.Add(vm);
            vm.IsCorrect = null;
        }

        _currentIndex = 0;
        OnPropertyChanged(nameof(CurrentTask));
    }
}