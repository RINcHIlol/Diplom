using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class CreatedLessonsViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly LessonsService _lessonsService;
    private readonly NavigationService _navigationService;
    private readonly MessageService _messageService;
    private readonly ModulesService _moduleService;
    
    public ObservableCollection<LessonShortDTO> Lessons { get; } = new();
    
    public ICommand GoLessonCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand GoCreateLessonCommand { get; }
    public ICommand GoEditLessonCommand { get; }
    public ICommand DeleteModuleCommand { get; }
    
    public CreatedLessonsViewModel(MainWindowViewModel main, SessionService session, LessonsService lessonsService, NavigationService navigationService,MessageService messageService, ModulesService modulesService)
    {
        _session = session;
        _main = main;
        _lessonsService = lessonsService;
        _navigationService = navigationService;
        _messageService = messageService;
        _moduleService = modulesService;
        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowCreatedModules();
        });
        
        GoLessonCommand = new RelayCommand<LessonShortDTO>(lesson =>
        {
            if (lesson != null)
            {
                _navigationService.CurrentLessonId = lesson.Id;
                _main.ShowCreatedTasks();
            }
        });
        
        GoCreateLessonCommand = new RelayCommand(() =>
        {
            _navigationService.CurrentLessonId = null;
            _main.ShowCreateLesson();
        });
        
        GoEditLessonCommand = new RelayCommand<LessonShortDTO>(lesson =>
        {
            _navigationService.CurrentLessonId = lesson.Id;
            _main.ShowCreateLesson();
        });
        
        DeleteModuleCommand = new RelayCommand(async () =>
        {
            var success = await _moduleService.DeleteModuleAsync(_navigationService.CurrentModuleId!.Value);

            if (success)
            {
                _messageService.SetMessage("ModulesUpdated");
                _main.ShowCreatedModules();
            }
        });

        _ = LoadLessonsAsync();
        _ = OnAppearingAsync();
    }
    
    private async Task LoadLessonsAsync()
    {
        var lessons = await _lessonsService.GetMyLessonsAsync(_navigationService.CurrentModuleId.Value);
        Lessons.Clear();
        
        foreach (var lesson in lessons)
            Lessons.Add(lesson);
    }
    
    public async Task OnAppearingAsync()
    {
        var msg = _messageService.GetMessage();

        if (msg == "LessonsUpdated")
        {
            await LoadLessonsAsync();
        }
    }
}