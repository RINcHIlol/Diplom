using System.Windows.Input;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class AuthViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;

    public ICommand GoBackCommand { get; }

    public AuthViewModel(MainWindowViewModel main)
    {
        _main = main;
        GoBackCommand = new RelayCommand(() =>
        {
            _main.CurrentView = new MainViewModel(_main);
        });
    }
}