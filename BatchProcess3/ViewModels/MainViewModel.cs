using System;
using BatchProcess3.Data;
using BatchProcess3.Factories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly PageFactory _pageFactory;

    [ObservableProperty]
    private DialogViewModel _currentDialog = new ConfirmDialogViewModel { IsDialogOpen = true };

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HomePageIsActive))]
    [NotifyPropertyChangedFor(nameof(ProcessPageIsActive))]
    [NotifyPropertyChangedFor(nameof(ActionsPageIsActive))]
    [NotifyPropertyChangedFor(nameof(MacrosPageIsActive))]
    [NotifyPropertyChangedFor(nameof(ReporterPageIsActive))]
    [NotifyPropertyChangedFor(nameof(HistoryPageIsActive))]
    [NotifyPropertyChangedFor(nameof(SettingsPageIsActive))]
    private PageViewModel _currentPage;

    [ObservableProperty]
    private bool _sideMenuExpanded;

    /// <summary>
    ///     Design-time only constructor
    /// </summary>
    // Allow nullable PageFactory for now in designer... ideally get it working
#pragma warning disable CS8618, CS9264
    public MainViewModel()
    {
        CurrentPage = new SettingsPageViewModel();
    }
#pragma warning restore CS8618, CS9264

    public MainViewModel(PageFactory pageFactory)
    {
        _pageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));

        CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Home);
    }

    public bool HomePageIsActive => IsPageActive(ApplicationPageNames.Home);
    public bool ProcessPageIsActive => IsPageActive(ApplicationPageNames.Process);
    public bool ActionsPageIsActive => IsPageActive(ApplicationPageNames.Actions);
    public bool MacrosPageIsActive => IsPageActive(ApplicationPageNames.Macros);
    public bool ReporterPageIsActive => IsPageActive(ApplicationPageNames.Reporter);
    public bool HistoryPageIsActive => IsPageActive(ApplicationPageNames.History);
    public bool SettingsPageIsActive => IsPageActive(ApplicationPageNames.Settings);

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
    }
}