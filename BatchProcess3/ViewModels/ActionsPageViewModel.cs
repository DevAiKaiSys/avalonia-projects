using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BatchProcess3.Data;
using BatchProcess3.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3.ViewModels;

/*public class ActionsPageViewModel : PageViewModel
{
    public ActionsPageViewModel()
    {
        PageName = ApplicationPageNames.Actions;
    }
}*/
public partial class ActionsPageViewModel(
    MainViewModel mainViewModel,
    DialogService dialogService,
    PrinterService printerService,
    DatabaseService databaseService)
    : PageViewModel(ApplicationPageNames.Actions)
{
    [ObservableProperty]
    private ObservableCollection<PrintSettingsViewModel> _printerSettings = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PrintListHasItems))]
    private ObservableCollection<ActionsTabPrintViewModel> _printList = [];

    /*[ObservableProperty]
    private ActionsPrintViewModel? _selectedPrintListItem;
    private ActionsPrinterProfileViewModel? _selectedPrinterProfileItem;
    public ActionsPrinterProfileViewModel? SelectedPrinterProfileItem
    {
        get => _selectedPrinterProfileItem;
        set
        {
            if (_selectedPrinterProfileItem != null && value == _selectedPrinterProfileItem)
                return;

            _selectedPrinterProfileItem = value;
        }
    }*/
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedPrintListItem))]
    private string _selectedPrintListItemId = "";

    // Design time only
    public ActionsPageViewModel() : this(new MainViewModel(), new DialogService(() => null), new PrinterService(),
        new DatabaseService(new ApplicationDbContext()))
    {
    }

    public ActionsTabPrintViewModel? SelectedPrintListItem =>
        PrintList.FirstOrDefault(f => f.Id == SelectedPrintListItemId);

    public bool PrintListHasItems => PrintList.Any();


    [RelayCommand]
    public void RefreshActionsPage(ActionsPageName actionsPageName)
    {
        switch (actionsPageName)
        {
            case ActionsPageName.Print: FetchPrintList(); break;
        }
    }

    [RelayCommand]
    private void FetchPrintSettings()
    {
        // Fetch live printers available on machine
        /*var availablePrinters = printerService.AvailablePrinters();

        var printerNameOptions = new ObservableCollection<KeyValuePair<string, string>>(
            availablePrinters.Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name))
        );*/

        /*var printerSettingsItem = new PrintSettingsProfileViewModel
        {
            Id = "2",
            Height = 200,
            Width = 140,
            ScaleToFit = true
            // PrinterNameOptions = printerNameOptions
        };

        var printerSettings = new ObservableCollection<PrintSettingsProfileViewModel>
        {
            printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem,
            printerSettingsItem,
            printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem,
            printerSettingsItem,
            printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem,
            printerSettingsItem,
            printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem,
            printerSettingsItem,
            printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem, printerSettingsItem,
            printerSettingsItem
        };

        _defaultPrinterSettings.PrinterSettings = printerSettings;

        PrinterSettings =
        [
            _defaultPrinterSettings,
            new PrintSettingsViewModel
            {
                Id = "1",
                Name = "Print Landscape",
                Description = "Print all files in landscape mode, 3 copies",
                Copies = 3,
                PrinterSettings = printerSettings
            },
            new PrintSettingsViewModel
            {
                Id = "2",
                Name = "Print Portrait",
                Description = "Print all files in portait mode",
                Copies = 1,
                PrinterSettings = printerSettings
            },
            new PrintSettingsViewModel
            {
                Id = "3",
                Name = "B&W A3",
                Description = "Make all A3 prints black and white",
                Copies = 5,
                PrinterSettings = printerSettings
            }
        ];*/
        var settings = databaseService.GetPrintSettings();

        /*PrinterSettings = new ObservableCollection<PrintSettingsViewModel>(settings.Select(f =>
            new PrintSettingsViewModel


            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                CanEdit = f.CanEdit,
                CanDelete = f.CanDelete,
                Copies = f.Copies,
                PrinterSettingProfiles = new ObservableCollection<PrintSettingsProfileViewModel>(
                    f.PrinterSettingProfiles.Select(profile => new PrintSettingsProfileViewModel


                    {
                        Id = profile.Id,
                        DrawingColor = new KeyValuePair<string, string>(profile.DrawingColor, profile.DrawingColor),
                        Height = profile.Height,
                        Orientation = new KeyValuePair<string, string>(profile.Orientation, profile.Orientation),
                        PaperSize = new KeyValuePair<string, string>(profile.PaperSize, profile.PaperSize),
                        PrinterName = new KeyValuePair<string, string>(profile.PrinterName, profile.PrinterName),
                        ScaleToFit = profile.ScaleToFit,
                        SourceTray = new KeyValuePair<string, string>(profile.SourceTray, profile.SourceTray),
                        Type = profile.Type,
                        Width = profile.Width
                    }))
            }));*/
        PrinterSettings = settings.ToViewModels();
    }

    [RelayCommand]
    private void FetchPrintList()
    {
        FetchPrintSettings();

        /*PrintList =
        [
            new ActionsTabPrintViewModel
            {
                Id = "1",
                JobName = "Print Only Drawings",
                Description = "Prints only drawing files",
                PrintDrawingRange = "0, 5, 7-8",
                PrintDrawings = true,
                DrawingExclusionList = $"Some item 1{Environment.NewLine}Some item 2{Environment.NewLine}Some item 3",
                PrinterSettingsId = "1"
            },
            new ActionsTabPrintViewModel
            {
                Id = "2", JobName = "Print All Drawings Scale To Fit",
                Description = "Prints drawing scaled to fit the paper", PrintDrawings = true, PrinterSettingsId = "2"
            },
            new ActionsTabPrintViewModel
            {
                Id = "3", JobName = "Print 3D Models A3", Description = "Prints models as 3D visuals",
                PrintModels = true, PrinterSettingsId = "3"
            }
        ];*/
        var printList = databaseService.GetPrintList();

        /*PrintList = new ObservableCollection<ActionsTabPrintViewModel>(printList.Select(f =>
            new ActionsTabPrintViewModel*/
        PrintList = new ObservableCollection<ActionsTabPrintViewModel>(printList
            .OrderBy(f => f.JobName)
            .Select(f => new ActionsTabPrintViewModel
            {
                Id = f.Id,
                JobName = f.JobName,
                Description = f.Description,
                DrawingExclusionIsWhiteList = f.DrawingExclusionIsWhiteList,
                DrawingExclusionList = f.DrawingExclusionList,
                PrintDrawingRange = f.PrintDrawingRange,
                PrintDrawings = f.PrintDrawings,
                PrinterSettingsId = f.PrinterSettingsId,
                PrintModels = f.PrintModels
            }));

        // Update PrintListHasItems when collection changes
        PrintList.CollectionChanged += (_, _) => OnPropertyChanged(nameof(PrintListHasItems));

        if (PrintList.Count > 0)
        {
            // Select first item
            SelectedPrintListItemId = PrintList.First().Id;

            // Store last fetched database save states
            foreach (var printItem in PrintList)
                printItem.SetSavedState();
        }
    }

    protected override void OnDesignTimeConstructor()
    {
        FetchPrintList();
    }

    [RelayCommand]
    private async Task DeletePrintSettingsAsync(string id)
    {
        if (PrinterSettings.Count(x => x.Id == id) != 1)
            // TODO: Throw/Warn?
            return;

        if (await DeletePrintSettingsFromUIAsync(id))
            databaseService.DeletePrintSettings(id);
    }

    [RelayCommand]
    private async Task DeletePrintItemAsync(string id)
    {
        // TODO: Pass this logic to a service that handles the database/storage/fetching
        //       For now just do it direct in here

        if (PrintList.Count(x => x.Id == id) != 1)
            // TODO: Throw/Warn?
            return;

        // Remove item
        /*PrintList.Remove(PrintList.First(x => x.Id == id));*/
        /*DeletePrintItemFromUI(id);*/
        /*await DeletePrintItemFromUIAsync(id);*/
        // If user selected to remove from UI (via Confirm dialog)
        if (await DeletePrintItemFromUIAsync(id))
            // Delete from database
            databaseService.DeletePrintListItem(id);
    }

    [RelayCommand]
    private async Task EditPrintSettingsAsync(string id)
    {
        // TODO: Pass this logic to a service that handles database etc...

        var profileViewModel = PrinterSettings.FirstOrDefault(f => f.Id == id);

        if (profileViewModel == null)
            // TODO: Throw/warn?
            return;

        // Copy view model
        var copiedProfileViewModel = new PrintSettingsViewModel();
        copiedProfileViewModel.RestoreState(profileViewModel.GetState());

        InjectPrinterDetails(copiedProfileViewModel);

        await dialogService.ShowDialog(mainViewModel, copiedProfileViewModel);

        // Ignore if we clicked cancel
        if (!copiedProfileViewModel.Confirmed)
            return;

        // TODO: Database stuff

        // Commit copied view model back
        profileViewModel.RestoreState(copiedProfileViewModel.GetState());
        databaseService.UpdatePrintSettings(copiedProfileViewModel.ToDataModel());
    }

    private void InjectPrinterDetails(PrintSettingsViewModel viewModel)
    {
        // Fetch live printers available on machine
        var availablePrinters = printerService.AvailablePrinters();

        var printerNameOptions = new ObservableCollection<KeyValuePair<string, string>>(
            availablePrinters.Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name))
        );

        foreach (var printerSettingsItem in viewModel.PrinterSettingProfiles)
        {
            printerSettingsItem.PrinterNameOptions = printerNameOptions;

            printerSettingsItem.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName != nameof(PrintSettingsProfileViewModel.PrinterName))
                    return;

                // Printer changed, update paper size and tray
                printerSettingsItem.PaperSizeOptions = new ObservableCollection<KeyValuePair<string, string>>(
                    availablePrinters.FirstOrDefault(f => f.Name == printerSettingsItem.PrinterName.Value)
                        ?.PaperSizes ?? []
                );

                printerSettingsItem.PaperSizeOptions.Insert(0,
                    new KeyValuePair<string, string>("(Default)", "(Default)"));

                printerSettingsItem.SourceTrayOptions = new ObservableCollection<KeyValuePair<string, string>>(
                    availablePrinters.FirstOrDefault(f => f.Name == printerSettingsItem.PrinterName.Value)
                        ?.SourceTrays ?? []
                );

                printerSettingsItem.SourceTrayOptions.Insert(0,
                    new KeyValuePair<string, string>("(Default)", "(Default)"));

                // Change paper size and source tray to first item
                /*printerSettingsItem.PaperSize = printerSettingsItem.PaperSizeOptions.FirstOrDefault();
                printerSettingsItem.SourceTray = printerSettingsItem.SourceTrayOptions.FirstOrDefault();*/
                if (!printerSettingsItem.PaperSizeOptions.Any(f => f.Value == printerSettingsItem.PaperSize.Value))
                    printerSettingsItem.PaperSize = printerSettingsItem.PaperSizeOptions.FirstOrDefault();

                if (!printerSettingsItem.SourceTrayOptions.Any(f => f.Value == printerSettingsItem.SourceTray.Value))
                    printerSettingsItem.SourceTray = printerSettingsItem.SourceTrayOptions.FirstOrDefault();
            };

            // Force a printer name change for initial list
            printerSettingsItem.RaiseOnPropertyChanged(nameof(printerSettingsItem.PrinterName));
        }

        // await dialogService.ShowDialog(mainViewModel, copiedProfileViewModel);
        //
        // // Ignore if we clicked cancel
        // if (!copiedProfileViewModel.Confirmed)
        //     return;
        //
        // // TODO: Database stuff
        //
        // // Commit copied view model back
        // profileViewModel.RestoreState(copiedProfileViewModel.GetState());
    }

    [RelayCommand]
    private void AddNewPrintItem()
    {
        // Fetch printer settings
        var printerSettings = databaseService.GetPrintSettings();

        // Create a new item
        var newItem = new ActionsTabPrintViewModel
        {
            Id = Guid.NewGuid().ToString("N"),
            IsSelected = true,
            IsNewItem = true,
            JobName = "New Print Item",
            PrinterSettingsId = printerSettings.FirstOrDefault()?.Id
        };

        // Add to the print list
        PrintList.Add(newItem);

        // Select item
        SelectedPrintListItemId = newItem.Id;
    }

    [RelayCommand]
    private async Task AddNewPrintSettingsAsync()
    {
        var confirmViewModel = new PrintSettingsViewModel
        {
            Name = "New Print Settings",
            PrinterSettingProfiles = databaseService.GetPrintSettingsProfiles().ToViewModels()
            // Title = "Printer settings",
            // Message = "",
            // DialogWidth = 500
            // OnConfirm = async (vm) =>
            // {
            //     await Task.Delay(2000);
            //
            //     vm.ProgressText = "This is taking a while...";
            //
            //     await Task.Delay(2000);
            //     
            //     vm.StatusText = "Oh no, something went wrong...";
            //
            //     return true;
            // }
        };

        // TODO: Remove once new confirm view model dialog is pulled from database
        confirmViewModel.RestoreState(confirmViewModel.GetState());

        InjectPrinterDetails(confirmViewModel);

        await dialogService.ShowDialog(mainViewModel, confirmViewModel);

        // Ignore if we clicked cancel
        if (!confirmViewModel.Confirmed)
            return;

        PrinterSettings.Add(confirmViewModel);
        databaseService.AddPrintSettings(confirmViewModel.ToDataModel());
    }

    [RelayCommand]
    private async Task CancelPrintItem()
    {
        // Ignore if nothing is selected
        if (SelectedPrintListItem == null)
            return;

        // If the selected item is new, delete it
        // Otherwise, restore from save state
        if (SelectedPrintListItem.IsNewItem)
            /*DeletePrintItemFromUI(SelectedPrintListItem.Id);*/
            await DeletePrintItemFromUIAsync(SelectedPrintListItem.Id, false);
        else
            SelectedPrintListItem.RestoreState();
    }

    [RelayCommand]
    private Task SavePrintItemAsync()
    {
        // Ignore if no selection
        if (SelectedPrintListItem == null)
            return Task.CompletedTask;

        // If the selected item is new...
        if (SelectedPrintListItem.IsNewItem)
            databaseService.AddPrintListItem(SelectedPrintListItem.ToDataModel());
        else
            databaseService.UpdatePrintListItem(SelectedPrintListItem.ToDataModel());

        // Flag new item as not new
        SelectedPrintListItem.IsNewItem = false;
        SelectedPrintListItem.SetSavedState();
        return Task.CompletedTask;
    }

    // ReSharper disable once InconsistentNaming
    private async Task<bool> DeletePrintSettingsFromUIAsync(string id, bool warn = true)
    {
        var index = PrinterSettings.IndexOf(PrinterSettings.First(x => x.Id == id));
        if (index == -1)
            return false;

        if (warn)
        {
            var confirmViewModel = new ConfirmDialogViewModel
            {
                Title = "Delete Print Profile?",
                Message = $"Are you sure you want to delete '{PrinterSettings[index].Name}'?",
                DialogWidth = 500
            };

            await dialogService.ShowDialog(mainViewModel, confirmViewModel);

            // Ignore if we clicked cancel
            if (!confirmViewModel.Confirmed)
                return false;
        }

        // Remove item
        PrinterSettings.RemoveAt(index);

        // Select the item below the deleted one
        if (index > 0) index--;

        if (PrinterSettings.Count > 0)
            SelectedPrintListItem!.PrinterSettingsId = PrinterSettings[index].Id;

        return true;
    }

    /*private void DeletePrintItemFromUI(string id)*/
    // ReSharper disable once InconsistentNaming
    private async Task<bool> DeletePrintItemFromUIAsync(string id, bool warn = true)
    {
        // Remove item
        var index = PrintList.IndexOf(PrintList.First(x => x.Id == id));
        if (index == -1)
            return false;

        if (warn)
        {
            var confirmViewModel = new ConfirmDialogViewModel
            {
                Title = "Delete Print Item?",
                Message = $"Are you sure you want to delete ' {PrintList[index].JobName}'?",
                DialogWidth = 500
                // For test
                /*OnConfirm = async vm =>
                {
                    await Task.Delay(2000);
                    vm.ProgressText = "This is taking a while...";
                    await Task.Delay(2000);

                    // For test fail
                    // vm.StatusText = "Oh no, something went wrong...";
                    // return false;

                    return true;
                }*/
            };

            await dialogService.ShowDialog(mainViewModel, confirmViewModel);

            // Ignore if we clicked cancel
            if (!confirmViewModel.Confirmed)
                return false;
        }

        // Remove item
        PrintList.RemoveAt(index);

        // Select the item below the deleted one
        if (index > 0) index--;

        if (PrintList.Count > 0)
            SelectedPrintListItemId = PrintList[index].Id;

        return true;
    }
}