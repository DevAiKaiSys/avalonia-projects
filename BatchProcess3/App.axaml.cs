using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using BatchProcess3.Data;
using BatchProcess3.Factories;
using BatchProcess3.Services;
using BatchProcess3.ViewModels;
using BatchProcess3.Views;
using Microsoft.Extensions.DependencyInjection;

[assembly: XmlnsDefinition("https://github.com/avaloniaui", "BatchProcess3.Controls")]

namespace BatchProcess3;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        collection.AddCommonServices();

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        var vm = services.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };

        base.OnFrameworkInitializationCompleted();
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<MainViewModel>();
        collection.AddTransient<HomePageViewModel>();
        collection.AddTransient<ProcessPageViewModel>();
        collection.AddTransient<ActionsPageViewModel>();
        collection.AddTransient<MacrosPageViewModel>();
        collection.AddTransient<ReporterPageViewModel>();
        collection.AddTransient<HistoryPageViewModel>();
        collection.AddTransient<SettingsPageViewModel>();

        collection.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(provider => pageName => pageName switch
        {
            ApplicationPageNames.Home => provider.GetRequiredService<HomePageViewModel>(),
            ApplicationPageNames.Process => provider.GetRequiredService<ProcessPageViewModel>(),
            ApplicationPageNames.Actions => provider.GetRequiredService<ActionsPageViewModel>(),
            ApplicationPageNames.Macros => provider.GetRequiredService<MacrosPageViewModel>(),
            ApplicationPageNames.Reporter => provider.GetRequiredService<ReporterPageViewModel>(),
            ApplicationPageNames.History => provider.GetRequiredService<HistoryPageViewModel>(),
            ApplicationPageNames.Settings => provider.GetRequiredService<SettingsPageViewModel>(),
            _ => throw new ArgumentException($"No ViewModel registered for page: {pageName}")
        });

        collection.AddSingleton<PageFactory>();
        collection.AddSingleton<DialogService>();

        collection.AddTransient<PrinterService>();

        // Database services
        collection.AddTransient<ApplicationDbContext>();
        collection.AddTransient<DatabaseService>();
        collection.AddSingleton<Func<DatabaseService>>(x => x.GetRequiredService<DatabaseService>);
        collection.AddSingleton<DatabaseFactory>();
    }
}