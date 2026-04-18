using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class CreateLessonViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly LessonsService _lessonsService;
    private readonly NavigationService _navigationService;
    
    private int? _lessonId;

    private string? _title;
    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    
    private string? _content;
    public string? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    private string? _errorMsg;
    public string? ErrorMsg
    {
        get => _errorMsg;
        set => SetProperty(ref _errorMsg, value);
    }
    
    private bool? _isError;
    public bool? IsError
    {
        get => _isError;
        set => SetProperty(ref _isError, value);
    }
    
    public bool IsEdit => _lessonId != null;

    public ICommand SaveCommand { get; }
    public ICommand BackCommand { get; }
    public CreateLessonViewModel(MainWindowViewModel main, SessionService session, LessonsService lessonsService, NavigationService navigationService)
    {
        _session = session;
        _main = main;
        _lessonsService = lessonsService;
        _navigationService = navigationService;
        
        _ = InitAsync();

        SaveCommand = new AsyncRelayCommand(SaveAsync);

        BackCommand = new RelayCommand(() =>
        {
            _main.ShowCreatedLessons();
        });
    }
    
    private async Task InitAsync()
    {
        if (_navigationService.CurrentModuleId.HasValue)
        {
            var lesson = await _lessonsService.GetLessonAsync(_navigationService.CurrentLessonId.Value);

            if (lesson != null)
            {
                _lessonId = lesson.Id;
                Title = lesson.Title;
                Content = lesson.Content;
            }
        }
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Content))
        {
            IsError = true;
            ErrorMsg = "Заполните все поля";
            return;
        }
        var dto = new CreateUpdateLessonDTO()
        {
            Title = Title!,
            Content = Content!
        };

        if (IsEdit)
        {
            await _lessonsService.UpdateLessonAsync(_navigationService.CurrentModuleId!.Value, _lessonId!.Value, dto);
        }
        else
        {
            await _lessonsService.CreateLessonAsync(_navigationService.CurrentModuleId!.Value, dto);
        }

        _main.ShowCreatedLessons();
    }
}