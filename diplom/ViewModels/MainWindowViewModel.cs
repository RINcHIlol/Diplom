using diplom.Models;
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

    public MainWindowViewModel(SessionService session, AuthService authService)
    {
        _session = session;
        _authService = authService;

        CurrentView = new MainViewModel(this, _session, authService);
        _ = TryRestoreSessionAsync();
    }

    private async Task TryRestoreSessionAsync()
    {
        _session.LoadToken();

        if (_session.Token == null)
            return;

        var user = await _authService.GetMeAsync(_session.Token);

        if (user != null)
            _session.Login(user, _session.Token);
        else
            _session.Logout();
    }

    public void Logout()
    {
        _session.Logout();
    }
}
