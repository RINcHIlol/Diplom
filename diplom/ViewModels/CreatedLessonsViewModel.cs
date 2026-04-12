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
    
    public ObservableCollection<LessonShortDTO> Lessons { get; } = new();
    
    public ICommand GoLessonCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand GoCreateLessonCommand { get; }
    public ICommand GoEditLessonCommand { get; }
    
    public CreatedLessonsViewModel(MainWindowViewModel main, SessionService session, LessonsService lessonsService, NavigationService navigationService)
    {
        _session = session;
        _main = main;
        _lessonsService = lessonsService;
        _navigationService = navigationService;
        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowCreatedModules();
        });
        
        GoLessonCommand = new RelayCommand<LessonShortDTO>(lesson =>
        {
            if (lesson != null)
            {
                _navigationService.CurrentLessonId = lesson.Id;
                // _main.ShowCreatedTasks();
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

        _ = LoadLessonsAsync();
    }
    
    private async Task LoadLessonsAsync()
    {
        var lessons = await _lessonsService.GetMyLessonsAsync(_navigationService.CurrentModuleId.Value);
        Lessons.Clear();
        
        foreach (var lesson in lessons)
            Lessons.Add(lesson);
    }
}