using System;
using BatchProcess3.Actions;
using BatchProcess3.DataStorage;
using BatchProcess3.Dialog;
using BatchProcess3.MainApp;
using BatchProcess3.Printer;
using BatchProcess3.SolidWorks;
using BatchProcess3.ViewModels;
using BatchProcess3.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using ActionPrintSettingsViewModel = BatchProcess3.ViewModels.Actions.ActionPrintSettingsViewModel;

namespace BatchProcess3.Bootstrap;

public static class Bootstrapper
{
    public static void RegisterCommonServices(IServiceCollection collection)
    {
        // Singleton Services
        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<HomePageViewModel>();
        collection.AddSingleton<DialogService>();

        // Page Factory Callback
        collection.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(provider => pageName => pageName switch
        {
            ApplicationPageNames.Home => provider.GetRequiredService<HomePageViewModel>(),
            ApplicationPageNames.Process => provider.GetRequiredService<ProcessPageViewModel>(),
            ApplicationPageNames.Actions => provider.GetRequiredService<ActionsPageViewModel>(),
            ApplicationPageNames.Macros => provider.GetRequiredService<MacrosPageViewModel>(),
            ApplicationPageNames.Reporter => provider.GetRequiredService<ReporterPageViewModel>(),
            ApplicationPageNames.History => provider.GetRequiredService<HistoryPageViewModel>(),
            ApplicationPageNames.Settings => provider.GetRequiredService<SettingsPageViewModel>(),
            ApplicationPageNames.Jobs => provider.GetRequiredService<JobsPageViewModel>(),
            _ => throw new ArgumentException($"No ViewModel registered for page: {pageName}")
        });

        // Page Factory
        collection.AddSingleton<PageFactory>();

        // Transient Services
        collection.AddTransient<ActionsPageViewModel>();
        collection.AddTransient<HistoryPageViewModel>();
        collection.AddTransient<MacrosPageViewModel>();
        collection.AddTransient<ProcessPageViewModel>();
        collection.AddTransient<ReporterPageViewModel>();
        collection.AddTransient<SettingsPageViewModel>();
        collection.AddTransient<JobsPageViewModel>();
        collection.AddTransient<ActionService>();
        collection.AddTransient<PrinterService>();
        collection.AddTransient<BatchProcessClient>();

        collection.AddTransient<ActionPrintSettingsViewModel>();
        collection.AddTransient<ConfirmDialogViewModel>();
        collection.AddTransient<ErrorViewModel>();

        // Database services
        collection.AddTransient<ApplicationDbContext>();
        collection.AddTransient<DatabaseService>();
        collection.AddSingleton<Func<DatabaseService>>(x => x.GetRequiredService<DatabaseService>);
        collection.AddSingleton<DatabaseFactory>();

        // Add Top Level Locator
        collection.AddTopLevelProvider();
    }
}