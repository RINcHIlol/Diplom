using System.Reflection;
using diplom.Services;
using System.Threading.Tasks;
using diplom.DTOs.Profile;
using diplom.ViewModels.Tasks;
using Task = System.Threading.Tasks.Task;
using User = diplom.ModelsApi.User;

namespace diplom.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentView;
    public ViewModelBase? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    private readonly SessionService _session;
    private readonly AuthService _authService;
    private readonly NavigationService _navigationService;
    
    private readonly ProfileService _profileService; 
    private readonly RegService _regService;
    private readonly CourseApiService _courseApiService;
    private readonly MessageService _messageService;
    private readonly ModulesService _modulesService;
    private readonly LessonsService _lessonsService;
    private readonly TaskService _taskService;
    private readonly ProgressService _progressService;
    
    public MainWindowViewModel(SessionService session, AuthService authService, ProfileService profileService, RegService regService, MessageService messageService, CourseApiService courseApiService, ModulesService modulesService, NavigationService navigationService, LessonsService lessonsService, TaskService taskService, ProgressService progressService)
    {
        _session = session;
        _authService = authService;
        _navigationService = navigationService;
        
        _profileService = profileService;
        _regService = regService;
        _messageService = messageService;
        _courseApiService = courseApiService;
        _modulesService = modulesService;
        _lessonsService = lessonsService;
        _taskService = taskService;
        _progressService = progressService;

        ShowMain();
        _ = TryRestoreSessionAsync();
    }

    private async Task TryRestoreSessionAsync()
    {
        _session.LoadToken();

        if (_session.Token == null)
            return;

        var user = await _authService.GetMeAsync();

        if (user != null)
        {
            _session.Login(user, _session.Token);
            ShowMain();   
        }
        else
            _session.Logout();
    }

    public void Logout()
    {
        _session.Logout();
    }

    public void ShowMain()
    {
        CurrentView = new MainViewModel(this, _session, _courseApiService, _navigationService);
    }

    public void ShowAuth()
    {
        CurrentView = new AuthViewModel(this, _session, _authService, _messageService);
    }

    public void ShowProfile()
    {
        CurrentView = new ProfileViewModel(this, _session, _profileService, _navigationService);
    }

    public void ShowReg()
    {
        CurrentView = new RegViewModel(this, _regService, _messageService);
    }

    public void ShowCourse()
    {
        CurrentView = new CourseViewModel(this, _session, _modulesService, _courseApiService, _navigationService);
    }

    public void ShowModule()
    {
        CurrentView = new ModulesViewModel(this, _session, _navigationService, _lessonsService, _modulesService);
    }

    public void ShowLesson()
    {
        CurrentView = new LessonViewModel(this, _session, _navigationService, _taskService, _progressService);
    }

    public async void ShowCreateTask()
    {
        var vm = new CreateTaskViewModel(this, _session, _navigationService, _taskService, _messageService);
        CurrentView = vm;

        await vm.InitAsync();
    }

    public void ShowCreatedCourses()
    {
        CurrentView = new CreatedCoursesViewModel(this, _session, _courseApiService, _navigationService, _messageService);
    }
    
    public void ShowCreateCourse()
    {
        CurrentView = new CreateCourseViewModel(this, _session, _courseApiService, _navigationService);
    }
    
    public void ShowCreatedModules()
    {
        CurrentView = new CreatedModulesViewModel(this, _session, _modulesService, _navigationService, _messageService, _courseApiService);
    }
    
    public void ShowCreateModule()
    {
        CurrentView = new CreateModuleViewModel(this, _session, _modulesService, _navigationService);
    }

    public void ShowCreatedLessons()
    {
        CurrentView = new CreatedLessonsViewModel(this, _session, _lessonsService, _navigationService, _messageService, _modulesService);
    }
    
    public void ShowCreateLesson()
    {
        CurrentView = new CreateLessonViewModel(this, _session, _lessonsService, _navigationService);
    }

    public void ShowCreatedTasks()
    {
        CurrentView = new CreatedTasksViewModel(this, _session, _navigationService, _taskService, _lessonsService, _messageService);
    }
    
    public void ShowEditProfile()
    {
        CurrentView = new EditProfileViewModel(this, _session, _authService);
    }
}
