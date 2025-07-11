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
    PrinterService printerService)
    : PageViewModel(ApplicationPageNames.Actions)
{
    // TODO: Remove once we have database service
    private readonly PrintSettingsViewModel _defaultPrinterSettings = new()
    {
        Id = "0", Name = "(Default)", Description = "Use all default settings", Copies = 1
        // TODO: Populate PrinterSettings
    };

    [ObservableProperty] private ObservableCollection<PrintSettingsViewModel> _printerSettings = [];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(PrintListHasItems))]
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
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(SelectedPrintListItem))]
    private string _selectedPrintListItemId = "";

    // Design time only
    public ActionsPageViewModel() : this(new MainViewModel(), new DialogService(() => null), new PrinterService())
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
            case ActionsPageName.Print: FetchPrintActionsData(); break;
        }
    }

    [RelayCommand]
    private void FetchPrintProfiles()
    {
        // Fetch live printers available on machine
        /*var availablePrinters = printerService.AvailablePrinters();

        var printerNameOptions = new ObservableCollection<KeyValuePair<string, string>>(
            availablePrinters.Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name))
        );*/

        // TODO: Pull from database 
        var printerSettingsItem = new PrintSettingsProfileViewModel
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
        ];
    }

    [RelayCommand]
    private void FetchPrintActionsData()
    {
        FetchPrintProfiles();

        // TODO: Fetch from a database/service provider
        PrintList =
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
        ];

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
        FetchPrintActionsData();
    }

    [RelayCommand]
    private async Task DeletePrintSettingsAsync(string id)
    {
        // TODO: Pass this logic to a service that handles the database/storage/fetching
        //       For now just do it direct in here

        if (PrinterSettings.Count(x => x.Id == id) != 1)
            // TODO: Throw/Warn?
            return;

        // TODO: Delete from database, then re-fetch to update UI
        //       1. Delete from database
        //       2. FetchPrintProfiles();

        await DeletePrintProfileFromUIAsync(id);
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
        await DeletePrintItemFromUIAsync(id);
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
    }

    private void InjectPrinterDetails(PrintSettingsViewModel viewModel)
    {
        // Fetch live printers available on machine
        var availablePrinters = printerService.AvailablePrinters();

        var printerNameOptions = new ObservableCollection<KeyValuePair<string, string>>(
            availablePrinters.Select(f => new KeyValuePair<string, string>(f.Id.ToString(), f.Name))
        );

        foreach (var printerSettingsItem in viewModel.PrinterSettings)
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

                printerSettingsItem.SourceTrayOptions = new ObservableCollection<KeyValuePair<string, string>>(
                    availablePrinters.FirstOrDefault(f => f.Name == printerSettingsItem.PrinterName.Value)
                        ?.SourceTrays ?? []
                );

                // Change paper size and source tray to first item
                printerSettingsItem.PaperSize = printerSettingsItem.PaperSizeOptions.FirstOrDefault();
                printerSettingsItem.SourceTray = printerSettingsItem.SourceTrayOptions.FirstOrDefault();
            };
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
        // TODO: Fetch new item defaults from a service provider
        // Create a new item
        var newItem = new ActionsTabPrintViewModel
        {
            Id = Guid.NewGuid().ToString("N"),
            IsSelected = true,
            IsNewItem = true,
            JobName = "New Print Item",
            PrinterSettingsId = "0"
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
            Name = "New Print Settings"
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

    // ReSharper disable once InconsistentNaming
    private async Task DeletePrintProfileFromUIAsync(string id, bool warn = true)
    {
        var index = PrinterSettings.IndexOf(PrinterSettings.First(x => x.Id == id));
        if (index == -1)
            return;

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
                return;
        }

        // Remove item
        PrinterSettings.RemoveAt(index);

        // Select the item below the deleted one
        if (index > 0) index--;

        if (PrinterSettings.Count > 0)
            SelectedPrintListItem!.PrinterSettingsId = PrinterSettings[index].Id;
    }

    /*private void DeletePrintItemFromUI(string id)*/
    // ReSharper disable once InconsistentNaming
    private async Task DeletePrintItemFromUIAsync(string id, bool warn = true)
    {
        // Remove item
        var index = PrintList.IndexOf(PrintList.First(x => x.Id == id));
        if (index == -1)
            return;

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
                return;
        }

        // Remove item
        PrintList.RemoveAt(index);

        // Select the item below the deleted one
        if (index > 0) index--;

        if (PrintList.Count > 0)
            SelectedPrintListItemId = PrintList[index].Id;
    }
}