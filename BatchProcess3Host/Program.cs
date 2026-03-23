using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BatchProcess3Host;

internal sealed class Program
{
    private static readonly CancellationTokenSource _cts = new();

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    /*public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);*/
    public static void Main(string[] args)
    {
        // Run Kestrel
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls("http://localhost:5000");

        var webApp = builder.Build();

        webApp.MapGet("/", () => "Hello World!");

        // Start kestrel on background thread
        Task.Run(() => webApp.RunAsync(_cts.Token));

        try
        {
            // Run Avalonia
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            // Close kestrel
            _cts.Cancel();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}