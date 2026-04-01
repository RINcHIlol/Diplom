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
    
    private string? _courseName;
    public string? CourseName
    {
        get => _courseName;
        set => SetProperty(ref _courseName, value);
    }
    
    private string? _courseDescription;
    public string? CourseDescription
    {
        get => _courseDescription;
        set => SetProperty(ref _courseDescription, value);
    }
    
    private double? _courseProgress;
    public double? CourseProgress
    {
        get => _courseProgress;
        set => SetProperty(ref _courseProgress, value);
    }
    
    public ICommand GoBackCommand { get; }

    public CourseViewModel(MainWindowViewModel main, CourseProgressDto course, SessionService session)
    {
        _main = main;
        _session = session;

        CourseName = course.Title;
        CourseDescription = course.Description;
        CourseProgress = course.ProgressPercent;
        
        GoBackCommand = new RelayCommand(() =>
        {
            // _main.CurrentView = new MainViewModel(_main, session, _authService);
            _main.ShowMain();
        });
    }
}