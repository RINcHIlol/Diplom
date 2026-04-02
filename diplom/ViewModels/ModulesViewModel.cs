using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;


public class ModulesViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly CourseProgressDto _course;

    public ModulesViewModel(MainWindowViewModel main, CourseProgressDto course, SessionService session)
    {
        _main = main;
        _session = session;
        _course = course;
    }
}
