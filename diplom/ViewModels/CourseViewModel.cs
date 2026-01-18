using System.Windows.Input;
using diplom.ModelsApi;
using diplom.ViewModels;

namespace diplom.ViewModels;

public class CourseViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;

    public ICommand GoBackCommand { get; }

    public CourseViewModel(MainWindowViewModel main, Course course)
    {
        _main = main;
        GoBackCommand = new RelayCommand(() =>
        {
            _main.CurrentView = new MainViewModel(_main);
        });
    }
}