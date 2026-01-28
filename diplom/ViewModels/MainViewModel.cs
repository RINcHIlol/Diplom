using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.ModelsApi;
using diplom.Services;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly HttpClient _httpClient;
    
    public ICommand GoAuthCommand { get; }
    public ICommand GoCourseCommand { get; }
    public ObservableCollection<Course> Courses { get; } = new();
    
    private readonly SessionService _session;
    private readonly CourseApiService _courseService;
    
    private readonly AuthService _authService;

    public bool IsAuthorized => _session.IsAuthorized;
    public User? CurrentUser => _session.CurrentUser;
    
    public string UserDisplayName =>
        _session.IsAuthorized
            ? _session.CurrentUser!.Login
            : "Guest";
    public string AuthButtonText => IsAuthorized ? "Выйти" : "Войти";
    
    public MainViewModel(MainWindowViewModel main, SessionService session, AuthService authService)
    {
        _session = session;
        _authService = authService;
        _main = main;
        _courseService = new CourseApiService("http://localhost:5132/");
        
        GoAuthCommand = new RelayCommand(() =>
        {
            if (_session.IsAuthorized)
                _main.Logout();
            else
                _main.CurrentView = new AuthViewModel(_main, _session, _authService);
        });
        
        GoCourseCommand = new RelayCommand<Course>(course =>
        {
            if (course != null)
                _main.CurrentView = new CourseViewModel(_main, course, session, _authService);
        });
        
        _ = LoadCoursesAsync();
    }
    
    private async Task LoadCoursesAsync()
    {
        var courses = await _courseService.GetCoursesAsync();
        Courses.Clear();
        foreach (var course in courses)
            Courses.Add(course);
    }
}