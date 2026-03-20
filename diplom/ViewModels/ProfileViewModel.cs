using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using API.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class ProfileViewModel: ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly ProfileService _profile;
    
    public ICommand GoBackCommand { get; }
    
    private string _login;
    public string Login
    {
        get => _login;
        set { _login = value; OnPropertyChanged(); }
    }

    private string _email;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    private int _xp;
    public int Xp
    {
        get => _xp;
        set { _xp = value; OnPropertyChanged(); }
    }
    public ObservableCollection<CourseProgressDto> Courses { get; set; } = new();

    public ProfileViewModel(MainWindowViewModel main, SessionService session, ProfileService profile)
    {
        _main = main;
        _session = session;
        _profile = profile;
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowMain();
        });
        
        LoadProfile();
    }
    
    private async void LoadProfile()
    {
        var profile = await _profile.GetProfileAsync();
        if (profile == null) return;

        Login = profile.Login;
        Email = profile.Email;
        Xp = profile.Xp;

        Courses.Clear();
        foreach (var course in profile.Courses)
        {
            Courses.Add(course);
        }
    }
}