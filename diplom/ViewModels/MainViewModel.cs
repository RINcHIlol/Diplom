using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.ModelsApi;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly HttpClient _httpClient;
    
    public ICommand GoAuthCommand { get; }
    public ICommand GoCourseCommand { get; }
    public ObservableCollection<Course> Courses { get; } = new();

    public MainViewModel(MainWindowViewModel main)
    {
        _main = main;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5132/")
        };
        GoAuthCommand = new RelayCommand(() =>
        {
            _main.CurrentView = new AuthViewModel(_main);
        });

        // GoCourseCommand = new RelayCommand(() =>
        //     {
        //         _main.CurrentView = new CourseViewModel(_main);
        //     }
        // );
        GoCourseCommand = new RelayCommand<Course>(course =>
        {
            if (course != null)
                _main.CurrentView = new CourseViewModel(_main, course);
        });
        
        _ = LoadCoursesAsync();
    }
    
    private async Task LoadCoursesAsync()
    {
        try
        {
            var courses = await _httpClient.GetFromJsonAsync<List<Course>>("api/courses");
            if (courses != null)
            {
                Courses.Clear();
                foreach (var course in courses)
                    Courses.Add(course);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
        }
    }
}