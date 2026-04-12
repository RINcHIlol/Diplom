using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class CreateCourseViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly CourseApiService _courseService;
    private readonly SessionService _sessionService;
    private readonly NavigationService _navigationService;

    private int? _courseId;

    private string? _title;
    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public bool IsEdit => _courseId != null;

    public ICommand SaveCommand { get; }
    public ICommand BackCommand { get; }

    public CreateCourseViewModel(
        MainWindowViewModel main,
        SessionService sessionService,
        CourseApiService courseService,
        NavigationService navigation)
    {
        _main = main;
        _sessionService = sessionService;
        _courseService = courseService;
        _navigationService = navigation;

        _ = InitAsync();

        SaveCommand = new AsyncRelayCommand(SaveAsync);

        BackCommand = new RelayCommand(() =>
        {
            _main.ShowCreatedCourses();
        });
    }
    
    private async Task InitAsync()
    {
        if (_navigationService.CurrentCourseId.HasValue)
        {
            var course = await _courseService.GetCourseByIdAsync(_navigationService.CurrentCourseId.Value);

            if (course != null)
            {
                _courseId = course.Id;
                Title = course.Title;
                Description = course.Description;
            }
        }
    }

    private async Task SaveAsync()
    {
        var dto = new CreateUpdateCourseDto
        {
            Title = Title!,
            Description = Description!
        };

        if (IsEdit)
        {
            await _courseService.UpdateCourseAsync(_courseId!.Value, dto);
        }
        else
        {
            await _courseService.CreateCourseAsync(dto);
        }

        _main.ShowCreatedCourses();
    }
}