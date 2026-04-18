using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class EditProfileViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly AuthService _authService;
    
    private string? _errorMsg;
    public string? ErrorMsg
    {
        get => _errorMsg;
        set => SetProperty(ref _errorMsg, value);
    }
    
    private bool? _isError;
    public bool? IsError
    {
        get => _isError;
        set => SetProperty(ref _isError, value);
    }
    
    private string? _login;
    public string? Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }
    
    private string? _email;
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }
    
    private string? _password;
    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }
    
    public ICommand SaveCommand { get; }
    public ICommand BackCommand { get; }
    public EditProfileViewModel(MainWindowViewModel main, SessionService session, AuthService authService)
    {
        _main = main;
        _session = session;
        _authService = authService;
        
        _ = InitAsync();

        SaveCommand = new AsyncRelayCommand(SaveAsync);

        BackCommand = new RelayCommand(() =>
        {
            _main.ShowProfile();
        });
    }
    
    private async Task InitAsync()
    {
        if (_session.IsAuthorized)
        {
            var lesson = _session.CurrentUser;

            if (lesson != null)
            {
                Login = lesson.Login;
                Email = lesson.Email;
                Password = lesson.Password_hash;
            }
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            if (Login == null || Email == null || Password == null)
            {
                ErrorMsg = "Заполните все поля";
                IsError = true;
                return;
            }
            
            if (Login.Length < 5 || !Email.Contains("@") || Password.Length < 5)
            {
                ErrorMsg = "Некорректные данные";
                IsError = true;
                return;
            }
            
            var dto = new RegRequest()
            {
                Login = Login!,
                Email = Email!,
                Password = Password!
            };

            if (await _authService.UpdateUserAsync(dto))
            {
                _main.ShowProfile();   
            }
            IsError = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}