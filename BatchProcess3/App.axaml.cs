using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using BatchProcess3.Bootstrap;
using BatchProcess3.Crash;
using BatchProcess3.ViewModels;
using BatchProcess3.ViewModels.Pages;
using BatchProcess3.Views;
using BatchProcess3.Views.Pages;
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
        /*new ErrorWindow { DataContext = new ErrorViewModel() }.Show();*/
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };

        // Get last crash data
        var lastCrash = CrashService.GetCrashData();

        // If we crashed the last time...
        if (lastCrash != null)
        {
            new ErrorWindow
            {
                DataContext = new ErrorViewModel
                {
                    Title = lastCrash.ErrorMessage,
                    Description = $"BatchProcess crashed at '{lastCrash.Source}'\r\n" +
                                  $"with the following error:\r\n\r\n" +
                                  $"{lastCrash.ErrorMessage}.\r\n\r\n" +
                                  $"Stack Trace:\r\n{lastCrash.StackTrace}"
                }
            }.Show();

            // Don't delete error log for 10 seconds
            Task.Delay(10000).ContinueWith(_ => CrashService.ClearCrashData());
        }

        base.OnFrameworkInitializationCompleted();
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Inject common services
        Bootstrapper.RegisterCommonServices(collection);
    }
}