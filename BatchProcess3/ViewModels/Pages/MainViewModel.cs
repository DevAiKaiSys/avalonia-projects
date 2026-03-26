using System;
using BatchProcess3.DataStorage;
using BatchProcess3.Interfaces;
using BatchProcess3.MainApp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3.ViewModels.Pages;

public partial class MainViewModel : ViewModelBase, IDialogProvider
{
    private readonly DatabaseFactory _databaseFactory;
    private readonly PageFactory _pageFactory;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageIsActive))]
    [NotifyPropertyChangedFor(nameof(ProcessPageIsActive))]
    [NotifyPropertyChangedFor(nameof(ActionsPageIsActive))]
    [NotifyPropertyChangedFor(nameof(MacrosPageIsActive))]
    [NotifyPropertyChangedFor(nameof(ReporterPageIsActive))]
    [NotifyPropertyChangedFor(nameof(HistoryPageIsActive))]
    [NotifyPropertyChangedFor(nameof(SettingsPageIsActive))]
    [NotifyPropertyChangedFor(nameof(JobsPageIsActive))]
    private PageViewModel _currentPage;

    [ObservableProperty]
    /*private DialogViewModel _currentDialog = new ConfirmDialogViewModel { IsDialogOpen = true };*/
    private DialogViewModel? _dialog;


    [ObservableProperty] private bool _sideMenuExpanded;

    public MainViewModel(PageFactory pageFactory, DatabaseFactory databaseFactory)
    {
        _pageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
        _databaseFactory = databaseFactory ?? throw new ArgumentNullException(nameof(databaseFactory));

        using var dbContext = _databaseFactory.GetDatabaseService();
        dbContext.ApplyMigrations();

        CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Settings);
    }

    public bool HomePageIsActive => IsPageActive(ApplicationPageNames.Home);
    public bool ProcessPageIsActive => IsPageActive(ApplicationPageNames.Process);
    public bool ActionsPageIsActive => IsPageActive(ApplicationPageNames.Actions);
    public bool MacrosPageIsActive => IsPageActive(ApplicationPageNames.Macros);
    public bool ReporterPageIsActive => IsPageActive(ApplicationPageNames.Reporter);
    public bool HistoryPageIsActive => IsPageActive(ApplicationPageNames.History);
    public bool SettingsPageIsActive => IsPageActive(ApplicationPageNames.Settings);
    public bool JobsPageIsActive => IsPageActive(ApplicationPageNames.Jobs);

    private bool IsPageActive(ApplicationPageNames pageName)
    {
        return CurrentPage.PageName == pageName;
    }

    [RelayCommand]
    private void SideMenuResize()
    {
        SideMenuExpanded = !SideMenuExpanded;
    }

    [RelayCommand]
    private void GoToPage(ApplicationPageNames pageName)
    {
        CurrentPage = _pageFactory.GetPageViewModel(pageName);

        // if (pageName == ApplicationPageNames.Actions)
        // {
        //     using var db = new ApplicationDbContext();
        //     db.Database.Migrate();
        //
        //     var setting = new SettingsDataModel
        //         { Id = Guid.NewGuid().ToString("N"), LocationPaths = ["Path 1", "Path 2", "Path 3"] };
        //
        //     db.Settings.Add(setting);
        //
        //     db.SaveChanges();
        //
        //     var allSettings = db.Settings.ToList();
        //
        //     foreach (var s in db.Settings)
        //         db.Settings.Remove(s);
        //
        //     db.SaveChanges();
        // }
    }
}