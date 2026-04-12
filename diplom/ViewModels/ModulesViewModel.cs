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
    private readonly ModulesService _modules;
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

    public ModulesViewModel(MainWindowViewModel main, SessionService session, NavigationService navigationService, LessonsService lessonsService, ModulesService modulesService)
    {
        _main = main;
        _session = session;
        _lesson = lessonsService;
        _modules = modulesService;
        _navigationService = navigationService;

        var moduleId = _navigationService.CurrentModuleId;
        if (moduleId != null)
        {
            _ = LoadLessonsAsync(moduleId.Value);
        }
        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowCourse();
        });

        GoLessonCommand = new RelayCommand<LessonProgressDto>(lesson =>
        {
            if (lesson != null)
            {
                _navigationService.CurrentLessonId = lesson.LessonId;
                _main.ShowLesson();
            }
        });
    }
    
    private async Task LoadLessonsAsync(int moduleId)
    {
        var module = await _modules.GetModuleAsync(moduleId);
        
        if (module != null)
        {
            ModuleName = module.Title;
        }
        
        Lessons = await _lesson.GetLessonsAsync(moduleId); 
        Lessons = Lessons.OrderBy(x => x.OrderIndex).ToList(); 
    }
}
