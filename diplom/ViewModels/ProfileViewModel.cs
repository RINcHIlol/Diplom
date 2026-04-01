using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using diplom.DTOs.Profile;
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
    
    private int _nextLvlXp;
    public int NextLvlXp
    {
        get => _nextLvlXp;
        set { _nextLvlXp = value; OnPropertyChanged(); }
    }
    
    private double _progress;
    public double Progress
    {
        get => _progress;
        set { _progress = value; OnPropertyChanged(); }
    }
    
    private Bitmap _levelImage;
    public Bitmap LevelImage
    {
        get => _levelImage;
        set { _levelImage = value; OnPropertyChanged(); }
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
        NextLvlXp = profile.NextLvlXp;
        Progress = profile.Progress;
        LevelImage = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/zvaniya/" + profile.CurrentLvl + ".png");

        Courses.Clear();
        foreach (var course in profile.Courses)
        {
            Courses.Add(course);
        }
    }
}