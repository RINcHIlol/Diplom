using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using diplom.Services;
using diplom.ViewModels;
using diplom.Views;
using Microsoft.Extensions.DependencyInjection;

namespace diplom;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // public override void OnFrameworkInitializationCompleted()
    // {
    //     var sessionService = new SessionService();
    //     var authService = new AuthService("http://localhost:5132/");
    //     var profileService = new ProfileService("http://localhost:5132/");
    //     var regService = new RegService("http://localhost:5132/");
    //     var messageService = new MessageService();
    //     
    //     if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    //     {
    //         DisableAvaloniaDataAnnotationValidation();
    //         // desktop.MainWindow = new MainWindow
    //         // {
    //         //     DataContext = new MainWindowViewModel(),
    //         // };
    //         var mainWindowVM = new MainWindowViewModel(sessionService, authService, profileService, regService, messageService);
    //         desktop.MainWindow = new MainWindow
    //         {
    //             DataContext = mainWindowVM
    //         };
    //     }
    //
    //     base.OnFrameworkInitializationCompleted();
    // }
    
    public override void OnFrameworkInitializationCompleted()
    {
        var services = Program.ConfigureServices();

        var sessionService = services.GetRequiredService<SessionService>();
        var mainWindowVM = services.GetRequiredService<MainWindowViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowVM
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}