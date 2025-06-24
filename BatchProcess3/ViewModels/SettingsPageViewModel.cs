using System.Collections.Generic;
using BatchProcess3.Data;
using BatchProcess3.Factories;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class SettingsPageViewModel : PageViewModel
{
    private readonly DatabaseFactory _factory;

    [ObservableProperty]
    private List<string> _locationPaths;

    /*public SettingsPageViewModel()*/
    public SettingsPageViewModel(DatabaseFactory databaseFactory) : base(ApplicationPageNames.Settings)
    {
        PageName = ApplicationPageNames.Settings;
        _factory = databaseFactory;

        // TEMP: Remove
        using var dbContext = _factory.GetDatabaseService();
        LocationPaths = dbContext.GetSettings()?.LocationPaths ?? new List<string>();
    }
}