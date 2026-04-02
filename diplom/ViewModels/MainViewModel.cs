using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
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
    public ICommand GoProfileCommand { get; }
    
    public ObservableCollection<CourseProgressDto> Courses { get; } = new();
    private string _searchBox = "";
    public string SearchBox{
        get => _searchBox;
        set
        {
            if (SetProperty(ref _searchBox, value))
            {
                _ = LoadCoursesAsync();
            }
        }
    }
    
    private readonly SessionService _session;
    private readonly CourseApiService _courseService;
    private readonly NavigationService _navigationService;
        
    public bool IsAuthorized => _session.IsAuthorized;
    public User? CurrentUser => _session.CurrentUser;
    
    public string UserDisplayName =>
        _session.IsAuthorized
            ? _session.CurrentUser!.Login
            : "Guest";
    public string AuthButtonText => IsAuthorized ? "Выйти" : "Войти";
    
    public MainViewModel(MainWindowViewModel main, SessionService session, CourseApiService courseService, NavigationService navigationService)
    {
        _session = session;
        _main = main;
        _courseService = courseService;
        _navigationService = navigationService;
        
        GoAuthCommand = new RelayCommand(async () =>
        {
            if (_session.IsAuthorized)
            {
                main.Logout();
                OnPropertyChanged(nameof(IsAuthorized));
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(UserDisplayName));
                OnPropertyChanged(nameof(AuthButtonText));
                
                await LoadCoursesAsync();
            }
            else
                _main.ShowAuth();
        });
        
        GoCourseCommand = new RelayCommand<CourseProgressDto>(course =>
        {
            if (course != null)
            {
                _navigationService.CurrentCourse = course;
                _main.ShowCourse();
            }
        });

        GoProfileCommand = new RelayCommand(() =>
        {
            _main.ShowProfile();
        });
        
        _ = LoadCoursesAsync();
    }
    
    private async Task LoadCoursesAsync()
    {
        var courses = await _courseService.GetCoursesAsync();
        Courses.Clear();

        if (!string.IsNullOrEmpty(SearchBox))
            courses = courses.Where(it => IsContains(SearchBox, it.Title)).ToList();
        foreach (var course in courses)
            Courses.Add(course);
    }

    public bool IsContains(string search, string name)
    {
        var _search = search.ToLower();
        var _name = name.ToLower();
        return _name.Contains(_search);
    }
}