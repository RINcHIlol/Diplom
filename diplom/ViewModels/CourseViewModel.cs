using System.Windows.Input;
using diplom.ModelsApi;
using diplom.Services;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class CourseViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly AuthService _authService;
    public ICommand GoBackCommand { get; }

    public CourseViewModel(MainWindowViewModel main, Course course, SessionService session, AuthService authService)
    {
        _main = main;
        _authService = authService;
        GoBackCommand = new RelayCommand(() =>
        {
            _main.CurrentView = new MainViewModel(_main, session, _authService);
        });
    }
}