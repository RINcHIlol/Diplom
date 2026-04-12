using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class CreatedModulesViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly ModulesService _modulesService;
    private readonly NavigationService _navigationService;
    
    public ObservableCollection<ModuleShortDTO> Modules { get; } = new();
    
    public ICommand GoModuleCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand GoCreateModuleCommand { get; }
    public ICommand GoEditModuleCommand { get; }
    
    public CreatedModulesViewModel(MainWindowViewModel main, SessionService session, ModulesService modulesService, NavigationService navigationService)
    {
        _session = session;
        _main = main;
        _modulesService = modulesService;
        _navigationService = navigationService;
        
        GoBackCommand = new RelayCommand(() =>
        {
            _main.ShowCreatedCourses();
        });
        
        GoModuleCommand = new RelayCommand<ModuleShortDTO>(module =>
        {
            if (module != null)
            {
                _navigationService.CurrentModuleId = module.Id;
                _main.ShowCreatedLessons();
            }
        });
        
        GoCreateModuleCommand = new RelayCommand(() =>
        {
            _navigationService.CurrentModuleId = null;
            _main.ShowCreateModule();
        });
        
        GoEditModuleCommand = new RelayCommand<ModuleShortDTO>(module =>
        {
            _navigationService.CurrentModuleId = module.Id;
            _main.ShowCreateModule();
        });

        _ = LoadModulesAsync();
    }
    
    private async Task LoadModulesAsync()
    {
        var modules = await _modulesService.GetMyModulesAsync(_navigationService.CurrentCourseId.Value);
        Modules.Clear();
        
        foreach (var course in modules)
            Modules.Add(course);
    }
}