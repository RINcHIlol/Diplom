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
    private readonly MessageService _messageService;
    
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
    private string _error = "";
    public string ErrorStr
    {
        get => _error;
        set => SetProperty(ref _error, value);
    }
    private bool _isError = false;
    public bool IsError
    {
        get => _isError;
        set => SetProperty(ref _isError, value);
    }
    
    private string? _message;
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
    
    public ICommand GoBackCommand { get; }
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public AuthViewModel(MainWindowViewModel main, SessionService session, AuthService authService, MessageService messageService)
    {
        _main = main;
        _session = session;
        _authService = authService;
        _messageService = messageService;
        _message = _messageService.GetMessage();

        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowMain();
        });

        LoginCommand = new AsyncRelayCommand(LoginAsync);

        RegisterCommand = new RelayCommand(() =>
        {
            _main.ShowReg();
        });
    }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorStr = "Заполните все данные!";
            IsError = true;
            return;
        }
        var (user, token) = await _authService.LoginAsync(Login, Password);
        if (user != null && token != null)
        {
            _session.Login(user, token);
            IsError = false;
            _main.ShowMain();
        }
        else
        {
            ErrorStr = "Неверные данные для входа";
            IsError = true;
        }
    }
}