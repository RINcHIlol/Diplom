using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Data.Converters;
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
    private readonly MessageService _messageService;
    
    private bool _isEdit;
    public bool IsEdit
    {
        get => _isEdit;
        set => SetProperty(ref _isEdit, value);
    }
    
    private string? _message;
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
    
    private string? _error;
    public string? Error
    {
        get => _error;
        set => SetProperty(ref _error, value);
    }
    public int EditingTaskId { get; set; }

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
    public ICommand BackCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand DeleteLessonCommand { get; }

    public CreateTaskViewModel(MainWindowViewModel mainWindowViewModel, SessionService session, NavigationService navigationService, TaskService taskService, MessageService messageService)
    {
        _taskService = taskService;
        _mainWindowViewModel = mainWindowViewModel;
        _navigationService = navigationService;
        _session = session;
        _messageService = messageService;

        SaveCommand = new RelayCommand(() =>
        {
            Save();
        });
        BackCommand = new RelayCommand(() =>
        {
            Reset();
            _mainWindowViewModel.ShowCreatedTasks();
        });
        
        DeleteLessonCommand = new RelayCommand(async () =>
        {
            var success = await _taskService.DeleteTaskAsync(_navigationService.CurrentTaskId!.Value);

            if (success)
            {
                Reset();
                Error = null;
                Message = null;
                
                _messageService.SetMessage("TasksUpdated");
                _mainWindowViewModel.ShowCreatedTasks();
            }
        });
    }

    private void ChangeType()
    {
        if (SelectedTask == null)
            return;
        
        Current = null;

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
        if (!Validate())
            return;

        var dto = Current!.BuildDto();
        dto.LessonId = _navigationService.CurrentLessonId!.Value;

        try
        {
            if (IsEdit)
            {
                await _taskService.UpdateTaskAsync(EditingTaskId, dto);
                Message = "Редактирование успешно";
            }
            else
            {
                await _taskService.CreateTaskAsync(dto);
                Message = "Задача создана";
            }

            await Task.Delay(600);

            Reset();
            Error = null;
            Message = null;
            _mainWindowViewModel.ShowCreatedTasks();
        }
        catch (Exception ex)
        {
            Error = "Ошибка сохранения: " + ex.Message;
        }
    }
    
    public async Task InitAsync()
    {
        if (_navigationService.CurrentTaskId == null)
        {
            Reset();
            return;
        }

        var task = await _taskService.GetByIdTaskAsync(_navigationService.CurrentTaskId.Value);

        if (task == null)
        {
            Reset();
            return;
        }

        IsEdit = true;
        EditingTaskId = task.Id;

        SelectedTask = TaskTypes.FirstOrDefault(x => x.Id == task.TaskTypeId);
        if (SelectedTask == null)
        {
            Reset();
            return;
        }

        ChangeType();
        Current?.LoadFromDto(task);
    }
    
    public void Reset()
    {
        IsEdit = false;
        EditingTaskId = 0;
        SelectedTask = null;
        Current = null;

        _navigationService.CurrentTaskId = null;

        OnPropertyChanged(nameof(IsEdit));
    }
    
    private bool Validate()
    {
        Error = null;

        if (Current == null)
        {
            Error = "Не выбран тип задания";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Current.Question))
        {
            Error = "Введите вопрос";
            return false;
        }

        var dto = Current.BuildDto();

        if (dto == null)
        {
            Error = "Ошибка формирования задания";
            return false;
        }

        if (dto.Answers == null || dto.Answers.Count == 0 && dto.TaskTypeId != 1)
        {
            Error = "Добавьте хотя бы один ответ";
            return false;
        }

        if (dto.Answers.Any(a => string.IsNullOrWhiteSpace(a.AnswerText)))
        {
            Error = "Есть пустые ответы";
            return false;
        }

        if (dto.TaskTypeId == 4)
        {
            if (dto.MatchingPairs == null || dto.MatchingPairs.Count == 0)
            {
                Error = "Добавьте пары для сопоставления";
                return false;
            }
        }

        if (dto.TaskTypeId == 5)
        {
            if (dto.Answers.Count < 2)
            {
                Error = "Для сортировки нужно минимум 2 элемента";
                return false;
            }
        }

        return true;
    }
    
    public void InitForEdit(int taskId)
    {
        EditingTaskId = taskId;
        IsEdit = true;
    }
    
    public void LoadTask(TaskDto task)
    {
        if (task == null)
            return;

        EditingTaskId = task.Id;
        IsEdit = true;

        SelectedTask = TaskTypes.FirstOrDefault(x => x.Id == task.TaskTypeId);

        if (SelectedTask == null)
            return;

        ChangeType();

        if (Current == null)
            return;

        Current.LoadFromDto(task);
    }
}