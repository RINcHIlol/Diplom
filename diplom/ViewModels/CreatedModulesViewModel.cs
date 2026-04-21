using System.Collections.ObjectModel;
using System.Linq;
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
    private readonly MessageService _messageService;
    private readonly CourseApiService _courseApiService;
    
    // public ObservableCollection<ModuleShortDTO> Modules { get; } = new();
    private ObservableCollection<ModuleShortDTO> _modules = new();
    public ObservableCollection<ModuleShortDTO> Modules
    {
        get => _modules;
        set => SetProperty(ref _modules, value);
    }
    
    public ICommand GoModuleCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand GoCreateModuleCommand { get; }
    public ICommand GoEditModuleCommand { get; }
    public ICommand DeleteCourseCommand { get; }
    
    public CreatedModulesViewModel(MainWindowViewModel main, SessionService session, ModulesService modulesService, NavigationService navigationService, MessageService messageService, CourseApiService courseApiService)
    {
        _session = session;
        _main = main;
        _modulesService = modulesService;
        _navigationService = navigationService;
        _messageService = messageService;
        _courseApiService = courseApiService;
        
        GoBackCommand = new RelayCommand(() =>
        {
            if (_navigationService.IsAdminMode)
            {
                _main.ShowAdmin();
            }
            else
            {
                _main.ShowCreatedCourses();   
            }
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
        
        DeleteCourseCommand = new RelayCommand(async () =>
        {
            var success = await _courseApiService.DeleteCourseAsync(_navigationService.CurrentCourseId!.Value);

            if (success)
            {
                _messageService.SetMessage("CoursesUpdated");
                _main.ShowCreatedCourses();
            }
        });

        _ = LoadModulesAsync();
        _ = OnAppearingAsync();
    }
    
    private async Task LoadModulesAsync()
    {
        var modules = await _modulesService.GetMyModulesAsync(_navigationService.CurrentCourseId.Value);
        // Modules.Clear();
        
        // foreach (var course in modules)
        //     Modules.Add(course);
        Modules = new ObservableCollection<ModuleShortDTO>(

            modules

                .OrderBy(x => x.OrderIndex)

                .ThenBy(x => x.Id)

        );
    }
    
    public async Task OnAppearingAsync()
    {
        var msg = _messageService.GetMessage();

        if (msg == "ModulesUpdated")
        {
            await LoadModulesAsync();
        }
    }
}