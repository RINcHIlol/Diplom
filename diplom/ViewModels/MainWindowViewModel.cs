using diplom.Services;
using System.Threading.Tasks;
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
    private readonly ProfileService _profileService; 
    private readonly RegService _regService;
    private readonly CourseApiService _courseApiService;
    private readonly MessageService _messageService;

    public MainWindowViewModel(SessionService session, AuthService authService, ProfileService profileService, RegService regService, MessageService messageService, CourseApiService courseApiService)
    {
        _session = session;
        _authService = authService;
        _profileService = profileService;
        _regService = regService;
        _messageService = messageService;
        _courseApiService = courseApiService;

        // CurrentView = new MainViewModel(this, _session, _authService, _profileService);
        ShowMain();
        _ = TryRestoreSessionAsync();
    }

    private async Task TryRestoreSessionAsync()
    {
        _session.LoadToken();

        if (_session.Token == null)
            return;

        // var user = await _authService.GetMeAsync(_session.Token);
        var user = await _authService.GetMeAsync();

        if (user != null)
            _session.Login(user, _session.Token);
        else
            _session.Logout();
    }

    public void Logout()
    {
        _session.Logout();
    }

    public void ShowMain()
    {
        CurrentView = new MainViewModel(this, _session, _authService, _profileService, _courseApiService);
    }

    public void ShowAuth()
    {
        CurrentView = new AuthViewModel(this, _session, _authService, _messageService);
    }

    public void ShowProfile()
    {
        CurrentView = new ProfileViewModel(this, _session, _profileService);
    }

    public void ShowReg()
    {
        // CurrentView = new RegViewModel(this, _session);
        CurrentView = new RegViewModel(this, _regService, _messageService);
    }
}
