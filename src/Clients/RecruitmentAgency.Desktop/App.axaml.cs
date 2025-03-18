using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecruitmentAgency.Desktop.Controls;
using RecruitmentAgency.Desktop.Services;
using RecruitmentAgency.Desktop.ViewModels;
using RecruitmentAgency.Desktop.Views;
using RecruitmentAgency.HttpClient;

namespace RecruitmentAgency.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var configuration = LoadConfiguration();
        var serviceProvider = ConfigureServices(configuration);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            desktop.MainWindow = serviceProvider.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();
    }
    
    private IServiceProvider ConfigureServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        var serverUrl = configuration["BaseServerUrl"] ??
                        throw new InvalidOperationException("Missing 'BaseServerUrl' in appsettings.json");

        services
            .AddSingleton(configuration)
            .AddScoped<Client>(f => new Client(serverUrl, new System.Net.Http.HttpClient()))
            .AddSingleton<UserContainer>();

        services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<MainWindow>();

        services
            .AddSingleton<LoginViewModel>()
            .AddSingleton<LoginControl>()
            .AddSingleton<AuthControl>()
            .AddSingleton<MiniProfileViewModel>()
            .AddSingleton<MiniProfileControl>()
            .AddSingleton<VacanciesViewModel>()
            .AddSingleton<VacanciesControl>()
            .AddTransient<JobApplicationsViewModel>()
            .AddTransient<JobApplicationsControl>()
            .AddTransient<EmployerVacanciesApplicationsViewModel>()
            .AddTransient<EmployerVacanciesApplicationsControl>();
        
        return services.BuildServiceProvider();
    }
}