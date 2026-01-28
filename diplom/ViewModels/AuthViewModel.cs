using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.Services;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class AuthViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly AuthService _authService;

    public AuthViewModel(MainWindowViewModel main, SessionService session, AuthService authService)
    {
        _main = main;
        _session = session;
        _authService = authService;

        GoBackCommand = new RelayCommand(() =>
        {
            _main.CurrentView = new MainViewModel(_main, _session, _authService);
        });

        LoginCommand = new AsyncRelayCommand(LoginAsync);
    }

    private string _login = "";
    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    private string _password = "";
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public ICommand GoBackCommand { get; }
    public ICommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        var (user, token) = await _authService.LoginAsync(Login, Password);
        if (user != null && token != null)
        {
            _session.Login(user, token);
            _main.CurrentView = new MainViewModel(_main, _session, _authService);
        }
        else
        {
            // Ошибка текстом
        }
    }
}