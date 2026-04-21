using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class AdminViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly CourseApiService _courseService;
    private readonly NavigationService _navigationService;
    private readonly MessageService _messageService;
    
    private ObservableCollection<CourseShortDto> _courses = new();
    public ObservableCollection<CourseShortDto> Courses
    {
        get => _courses;
        set => SetProperty(ref _courses, value);
    }
    
    public ICommand GoCourseCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand GoCreateCourseCommand { get; }
    public ICommand GoEditCourseCommand { get; }

    public AdminViewModel(MainWindowViewModel main, SessionService session, CourseApiService courseService, NavigationService navigationService, MessageService messageService)
    {
        _session = session;
        _main = main;
        _courseService = courseService;
        _navigationService = navigationService;
        _messageService = messageService;
        
        GoBackCommand = new RelayCommand(() =>
        {
            _navigationService.IsAdminMode = false;
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
        _ = OnAppearingAsync();
    }
    
    private async Task LoadCoursesAsync()
    {
        var courses = await _courseService.GetCoursesForAdminAsync();
        Courses = new ObservableCollection<CourseShortDto>(

            courses

                .OrderBy(x => x.Id)
            
        );
    }
    
    public async Task OnAppearingAsync()
    {
        var msg = _messageService.GetMessage();

        if (msg == "CoursesUpdated")
        {
            await LoadCoursesAsync();
        }
    }
}