using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.Services;

namespace diplom.ViewModels;

// public class RegViewModel : ViewModelBase
// {
//     
//     public RegViewModel(MainWindowViewModel main, SessionService session)
//     {
//         
//     }
// }

public class RegViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    // private readonly SessionService _session;
    // private readonly AuthService _authService;
    private readonly RegService _regService;
    private readonly MessageService _messageService;
    
    private string _login = "";
    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }
    private string _email = "";
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
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

    public ICommand GoBackCommand { get; }
    public ICommand RegisterCommand { get; }

    // public RegViewModel(MainWindowViewModel main, SessionService session)
    public RegViewModel(MainWindowViewModel main, RegService regService, MessageService messageService)
    {
        _main = main;
        _regService = regService;
        _messageService = messageService;
        // _session = session;

        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowAuth();
        });
        
        RegisterCommand = new AsyncRelayCommand(RegAsync);
    }

    private async Task RegAsync()
    {
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
        {
            ErrorStr = "Заполните все данные!";
            IsError = true;
            return;
        }
        var (success, error) = await _regService.RegAsync(Login, Email, Password);
        if (success)
        {
            IsError = false;
            _messageService.SetMessage("Регистарция успешна!");
            _main.ShowAuth();
        }
        else
        {
            ErrorStr = error;
            IsError = true;
        }
    }
}