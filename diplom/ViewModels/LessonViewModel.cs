using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.Services;
using diplom.ViewModels.Tasks;

namespace diplom.ViewModels;

public class LessonViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly NavigationService _navigation;
    private readonly TaskService _taskService;

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

    public LessonViewModel(MainWindowViewModel main, SessionService session, NavigationService navigation, TaskService taskService)
    {
        _main = main;
        _session = session;
        _navigation = navigation;
        _taskService = taskService;

        GoBackCommand = new RelayCommand(() => _main.ShowModule());
        SubmitCommand = new RelayCommand(SubmitCurrentTask);

        LoadTasksAsync();
    }

    private async void LoadTasksAsync()
    {
        if (_navigation.CurrentLesson == null) return;

        LessonName = _navigation.CurrentLesson.Title;

        var taskViewModels = await _taskService.GetTasksAsync(_navigation.CurrentLesson.LessonId);

        foreach (var vm in taskViewModels)
            Tasks.Add(vm);

        _currentIndex = 0;

        _lessonCompleted = await _taskService.IsLessonCompletedAsync(_navigation.CurrentLesson.LessonId);

        if (_lessonCompleted)
        {
            foreach (var t in Tasks)
                t.IsCorrect = true;
        }

        OnPropertyChanged(nameof(CurrentTask));
    }

    private void CheckLessonCompletion()
    {
        if (_lessonCompleted) return;

        bool allAnswered = Tasks.All(t => t.IsCorrect != null);

        if (!allAnswered)
        {
            if (_currentIndex == Tasks.Count - 1)
            {
                ResultText = "⚠️ Есть нерешённые задания";
            }
            return;
        }

        bool allCorrect = Tasks.All(t => t.IsCorrect == true);

        if (allCorrect)
        {
            _lessonCompleted = true; 
            _main.ShowModule();
        }
    }

    private async void SubmitCurrentTask()
    {
        if (CurrentTask == null) return;

        if (_lessonCompleted)
        {
            ResultText = "Урок уже пройден ✅";
            return;
        }

        List<int> answerIds = new();

        if (CurrentTask is SingleChoiceTaskViewModel single)
        {
            var selected = single.Answers.FirstOrDefault(a => a.IsSelected);
            if (selected != null)
                answerIds.Add(selected.Id);
        }
        else if (CurrentTask is MultipleChoiceTaskViewModel multi)
        {
            answerIds = multi.Answers.Where(a => a.IsSelected).Select(a => a.Id).ToList();
        }

        string? textAnswer = CurrentTask is TextTaskViewModel text ? text.UserAnswer : null;

        bool isCorrect = await _taskService.SubmitAsync(CurrentTask.Id, answerIds, textAnswer);

        CurrentTask.IsCorrect = isCorrect;

        ResultText = isCorrect ? "✅ Правильно" : "❌ Неправильно";

        CheckLessonCompletion();
    }
}