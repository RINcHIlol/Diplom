using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace diplom.ViewModels;

public class CreatedCoursesViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly CourseApiService _courseService;
    private readonly NavigationService _navigationService;
    
    public ObservableCollection<CourseShortDto> Courses { get; } = new();
    
    public ICommand GoCourseCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand GoCreateCourseCommand { get; }
    public ICommand GoEditCourseCommand { get; }

    public CreatedCoursesViewModel(MainWindowViewModel main, SessionService session, CourseApiService courseService, NavigationService navigationService)
    {
        _session = session;
        _main = main;
        _courseService = courseService;
        _navigationService = navigationService;
        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowMain();
        });
        
        GoCourseCommand = new RelayCommand<CourseShortDto>(course =>
        {
            if (course != null)
            {
                _navigationService.CurrentCourseId = course.Id;
                _main.ShowCreatedModules();
            }
        });
        
        GoCreateCourseCommand = new RelayCommand(() =>
        {
            _navigationService.CurrentCourseId = null;
            _main.ShowCreateCourse();
        });
        
        GoEditCourseCommand = new RelayCommand<CourseShortDto>(course =>
        {
            _navigationService.CurrentCourseId = course.Id;
            _main.ShowCreateCourse();
        });
        
        _ = LoadCoursesAsync();
    }
    
    private async Task LoadCoursesAsync()
    {
        var courses = await _courseService.GetCoursesForCreatorAsync();
        Courses.Clear();
        
        foreach (var course in courses)
            Courses.Add(course);
    }
}