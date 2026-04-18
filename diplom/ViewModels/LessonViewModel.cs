using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;
using diplom.ViewModels.Tasks;
using TaskFactory = diplom.Services.TaskFactory;

namespace diplom.ViewModels;

public class LessonViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly NavigationService _navigation;
    private readonly TaskService _taskService;
    private readonly ProgressService _progressService;

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
    public ICommand SubmitCommand { get; }

    private bool _lessonCompleted;

    public LessonViewModel(MainWindowViewModel main, SessionService session, NavigationService navigation, TaskService taskService, ProgressService progressService)
    {
        _main = main;
        _session = session;
        _navigation = navigation;
        _taskService = taskService;
        _progressService = progressService;

        GoBackCommand = new RelayCommand(() => _main.ShowModule());
        SubmitCommand = new RelayCommand(SubmitCurrentTask);

        LoadTasksAsync();
    }

    private async void LoadTasksAsync()
    {
        if (_navigation.CurrentLessonId == null)
            return;

        Tasks.Clear();

        var lesson = _navigation.CurrentLessonId.Value;

        var taskVMs = await _taskService.GetTasksAsync(lesson);
        var progress = await _taskService.GetTaskProgressAsync(lesson);

        var progressMap = progress.ToDictionary(x => x.TaskId);

        var orderedTasks = taskVMs
            .OrderBy(x => x.OrderIndex)
            .ToList();
            
        for (int i = 0; i < orderedTasks.Count; i++)
        {
            var vm = orderedTasks[i];
            
            if (progressMap.TryGetValue(vm.Id, out var p))
                vm.IsCorrect = p.IsCorrect;   
            else
                vm.IsCorrect = null;        
            vm.DisplayIndex = i + 1;
            
            Tasks.Add(vm);
        }

        _currentIndex = 0;
        OnPropertyChanged(nameof(CurrentTask));
    }
    
    private void CheckLessonCompletion()
    {
        if (_lessonCompleted) return;

        if (Tasks.Any(t => t.IsCorrect != true))
            return;

        if (Tasks.All(t => t.IsCorrect == true))
        {
            _lessonCompleted = true;
            _main.ShowModule();
        }
    }

    private async void SubmitCurrentTask()
    {
        if (CurrentTask == null) return;
        if (CurrentTask.IsCorrect == true) return;

        if (_lessonCompleted)
        {
            ResultText = "Урок уже пройден ✅";
            return;
        }

        var answerIds = CurrentTask.GetAnswerIds();
        var textAnswer = CurrentTask.GetTextAnswer();
        var matches = CurrentTask.GetMatches();

        bool isCorrect = await _taskService.SubmitAsync(
            CurrentTask.Id,
            answerIds,
            textAnswer,
            matches
        );

        if (isCorrect)
        {
            await _progressService.UpdateXpAsync(10);
        }

        CurrentTask.IsCorrect = isCorrect;
        ResultText = isCorrect ? "✅ Правильно" : "❌ Неправильно";

        CheckLessonCompletion();
    }
}