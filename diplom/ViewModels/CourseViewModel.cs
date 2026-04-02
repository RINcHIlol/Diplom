using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
    //
    // private string? _courseDescription;
    // public string? CourseDescription
    // {
    //     get => _courseDescription;
    //     set => SetProperty(ref _courseDescription, value);
    // }
    //
    // private double? _totalLessons;
    // public double? TotalLessons
    // {
    //     get => _totalLessons;
    //     set => SetProperty(ref _totalLessons, value);
    // }
    //
    // private double? _completedLessons;
    // public double? CompletedLessons
    // {
    //     get => _completedLessons;
    //     set => SetProperty(ref _completedLessons, value);
    // }
    //
    // private double? _courseProgress;
    // public double? CourseProgress
    // {
    //     get => _courseProgress;
    //     set => SetProperty(ref _courseProgress, value);
    // }
    
    public ICommand GoBackCommand { get; }
    public ICommand GoModuleCommand { get; }

    public CourseViewModel(MainWindowViewModel main, CourseProgressDto course, SessionService session, ModulesService modulesService)
    {
        _main = main;
        _session = session;
        _module = modulesService;

        _ = LoadModulesAsync(course.CourseId);

        CourseName = course.Title;
        // CourseDescription = course.Description;
        // CompletedLessons = course.CompletedLessons;
        // TotalLessons = course.TotalLessons;
        // CourseProgress = course.ProgressPercent;
        
        GoBackCommand = new RelayCommand(() =>
        {
            // _main.CurrentView = new MainViewModel(_main, session, _authService);
            _main.ShowMain();
        });

        GoModuleCommand = new RelayCommand(() =>
        {
            Console.WriteLine("popa");
        });
    }
    
    private async Task LoadModulesAsync(int courseId)
    {
        Modules = await _module.GetModulesAsync(courseId);
        Modules.OrderBy(x => x.OrderIndex).ToList();
    }
}