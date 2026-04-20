using Avalonia;
using System;
using diplom.Services;
using diplom.Services.Handlers;
using diplom.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace diplom;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<SessionService>();
        services.AddSingleton<MessageService>();
        services.AddSingleton<NavigationService>();

        services.AddTransient<AuthHandler>();

        services.AddHttpClient("Api", client =>
            {
                // client.BaseAddress = new Uri("http://localhost:5132/");
                client.BaseAddress = new Uri("http://72.56.39.63:5132/");
            })
            .AddHttpMessageHandler<AuthHandler>();

        services.AddTransient<AuthService>();
        services.AddTransient<CourseApiService>();
        services.AddTransient<ProfileService>();
        services.AddTransient<RegService>();
        services.AddTransient<ModulesService>();
        services.AddTransient<LessonsService>();
        services.AddTransient<TaskService>();
        services.AddTransient<ProgressService>();

        services.AddTransient<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }
}