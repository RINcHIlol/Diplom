using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using diplom.DTOs.Profile;
using diplom.Services;

namespace diplom.ViewModels;

public class CreateModuleViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly SessionService _session;
    private readonly ModulesService _modulesService;
    private readonly NavigationService _navigationService;
    
    private int? _moduleId;

    private string? _title;
    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool IsEdit => _moduleId != null;

    public ICommand SaveCommand { get; }
    public ICommand BackCommand { get; }
    public CreateModuleViewModel(MainWindowViewModel main, SessionService session, ModulesService modulesService, NavigationService navigationService)
    {
        _session = session;
        _main = main;
        _modulesService = modulesService;
        _navigationService = navigationService;
        
        _ = InitAsync();

        SaveCommand = new AsyncRelayCommand(SaveAsync);

        BackCommand = new RelayCommand(() =>
        {
            _main.ShowCreatedModules();
        });
    }
    
    private async Task InitAsync()
    {
        if (_navigationService.CurrentModuleId.HasValue)
        {
            var module = await _modulesService.GetModuleAsync(_navigationService.CurrentModuleId.Value);

            if (module != null)
            {
                _moduleId = module.Id;
                Title = module.Title;
            }
        }
    }

    private async Task SaveAsync()
    {
        var dto = new CreateUpdateModuleDto()
        {
            Title = Title!,
        };

        if (IsEdit)
        {
            await _modulesService.UpdateModuleAsync(_navigationService.CurrentCourseId!.Value, _moduleId!.Value, dto);
        }
        else
        {
            await _modulesService.CreateModuleAsync(_navigationService.CurrentCourseId!.Value, dto);
        }

        _main.ShowCreatedModules();
    }
}