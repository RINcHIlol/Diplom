
using System.ComponentModel;
using System.IO;
using diplom.ModelsApi;

namespace diplom.Services;

// public class SessionService
// {
//     private const string TokenFile = "session.token";
//
//     public User? CurrentUser { get; private set; }
//     public string? Token { get; private set; }
//
//     public bool IsAuthorized => CurrentUser != null;

public class SessionService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private const string TokenFile = "session.token";

    private User? _currentUser;
    public User? CurrentUser
    {
        get => _currentUser;
        private set
        {
            _currentUser = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUser)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAuthorized)));
        }
    }

    private string? _token;
    public string? Token
    {
        get => _token;
        private set
        {
            _token = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Token)));
        }
    }

    public bool IsAuthorized => CurrentUser != null;

    public void Login(User user, string token)
    {
        CurrentUser = user;
        Token = token;

        File.WriteAllText(TokenFile, token);
    }

    public void Logout()
    {
        CurrentUser = null;
        Token = null;

        if (File.Exists(TokenFile))
            File.Delete(TokenFile);
    }

    public void LoadToken()
    {
        if (File.Exists(TokenFile))
        {
            Token = File.ReadAllText(TokenFile);
        }
    }
}