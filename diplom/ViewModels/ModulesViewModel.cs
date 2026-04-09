using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;


public class ModulesViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly LessonsService _lesson;
    private readonly NavigationService _navigationService;
    
    private List<LessonProgressDto>? _lessons;
    public List<LessonProgressDto>? Lessons
    {
        get => _lessons;
        set => SetProperty(ref _lessons, value);
    }
    
    private string? _moduleName;
    public string? ModuleName
    {
        get => _moduleName;
        set => SetProperty(ref _moduleName, value);
    }
    
    public ICommand GoBackCommand { get; }
    public ICommand GoLessonCommand { get; }

    public ModulesViewModel(MainWindowViewModel main, SessionService session, NavigationService navigationService, LessonsService lessonsService)
    {
        _main = main;
        _session = session;
        _lesson = lessonsService;
        _navigationService = navigationService;

        var module = _navigationService.CurrentModule;
        if (module != null)
        {
            ModuleName = module.Title;
            _ = LoadLessonsAsync(module.ModuleId);
        }
        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowCourse();
        });

        GoLessonCommand = new RelayCommand<LessonProgressDto>(lesson =>
        {
            if (lesson != null)
            {
                _navigationService.CurrentLesson = lesson;
                _main.ShowLesson();
            }
        });
    }
    
    private async Task LoadLessonsAsync(int moduleId)
    {
        Lessons = await _lesson.GetLessonsAsync(moduleId); 
        Lessons = Lessons.OrderBy(x => x.OrderIndex).ToList(); 
    }
}
