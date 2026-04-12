using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.ModelsApi;
using diplom.Services;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class CourseViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly ModulesService _module;
    private readonly CourseApiService _course;
    private readonly NavigationService _navigationService;
    
    private List<ModuleProgressDto>? _modules;
    public List<ModuleProgressDto>? Modules
    {
        get => _modules;
        set => SetProperty(ref _modules, value);
    }
    
    private string? _courseName;
    public string? CourseName
    {
        get => _courseName;
        set => SetProperty(ref _courseName, value);
    }
    
    public ICommand GoBackCommand { get; }
    public ICommand GoModuleCommand { get; }

    public CourseViewModel(MainWindowViewModel main, SessionService session, ModulesService modulesService, CourseApiService courseService, NavigationService navigationService)
    {
        _main = main;
        _session = session;
        _module = modulesService;
        _course = courseService;
        _navigationService = navigationService;

        var courseId = _navigationService.CurrentCourseId;

        if(courseId != null)
        {
            _ = LoadDataAsync(courseId.Value);
        }

        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowMain();
        });

        GoModuleCommand = new RelayCommand<ModuleProgressDto>(module =>
        {
            if (module != null && _session.IsAuthorized)
            {
                _navigationService.CurrentModuleId = module.ModuleId;
                _main.ShowModule();
            }
            else
            {
                _main.ShowAuth();
            }
        });
    }
    
    private async Task LoadDataAsync(int courseId)
    {
        var course = await _course.GetCourseByIdAsync(courseId);

        if (course != null)
        {
            CourseName = course.Title;
        }

        var modules = await _module.GetModulesAsync(courseId);

        Modules = modules
            .OrderBy(x => x.OrderIndex)
            .ToList();
    }
}