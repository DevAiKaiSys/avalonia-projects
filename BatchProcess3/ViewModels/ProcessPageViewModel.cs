using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using BatchProcess3.DataStorage;
using BatchProcess3.DataStorage.DataModels;
using BatchProcess3.Dialog;
using BatchProcess3.MainApp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3.ViewModels;

public partial class ProcessPageViewModel : PageViewModel
{
    #region Commands

    [RelayCommand]
    private void AddActionToProcess(AvailableActionItemViewModel item)
    {
        if (ProcessList.SelectedItem == null) return;

        if (item.ActionViewModel == null) return;

        var copy = new AvailableActionItemViewModel();
        copy.RestoreState(item.GetState());

        // Make the Id start with the process Id
        if (copy.ActionViewModel != null)
            copy.ActionViewModel.Id = $"{ProcessList.SelectedItemId}:{item.ActionViewModel.Id}";

        ProcessList.SelectedItem.Actions.Add(copy.ActionViewModel!);
    }

    #endregion

    #region Members

    private DatabaseService _databaseService;
    private MainViewModel _mainViewModel;
    private DialogService _dialogService;

    #endregion

    #region Properties

    [ObservableProperty]
    private SelectableItemListViewModel<ProcessViewModel> _processList;

    [ObservableProperty]
    private ObservableCollection<AvailableActionItemViewModel> _availableActionsList;

    #endregion

    #region Constructor

    public ProcessPageViewModel(
        MainViewModel mainViewModel,
        DialogService dialogService,
        DatabaseService databaseService) : base(ApplicationPageNames.Process)
    {
        Initialize(mainViewModel, dialogService, databaseService);
    }

    private void Initialize(MainViewModel mainViewModel, DialogService dialogService, DatabaseService databaseService)
    {
        _mainViewModel = mainViewModel;
        _dialogService = dialogService;
        _databaseService = databaseService;

        ProcessList = new SelectableItemListViewModel<ProcessViewModel>(
            "Process",
            mainViewModel,
            dialogService,
            () =>
            {
                var list = databaseService.GetProcessList();

                return new ObservableCollection<ProcessViewModel>(list
                    .OrderBy(f => f.JobName)
                    .Select(f => f.ToViewModel()));
            },
            () => new ProcessViewModel
            {
                Id = Guid.NewGuid().ToString("N"), IsNewItem = true, JobName = "New Process"
            },
            databaseService.DeleteProcessItem,
            item => databaseService.AddProcessItem(item.ToDataModel()),
            item => databaseService.UpdateProcessItem(item.ToDataModel())
        );

        List<AvailableActionItemViewModel> ToAvailableActionList<T>(string category, List<T> list)
            where T : ActionDataModel
        {
            var returnList = new List<AvailableActionItemViewModel>
            {
                // Add header
                new() { Category = category }
            };

            // Add items
            returnList.AddRange(list.Select(f => new AvailableActionItemViewModel
            {
                ActionViewModel = f.ToViewModel(),
                Category = category
            }));

            // Edit all the Id's
            returnList.ForEach(f =>
            {
                if (f.ActionViewModel != null)
                    f.ActionViewModel.Id = $"{f.ActionViewModel.SortOrder}:{f.ActionViewModel.Id}";
            });

            return returnList;
        }

        var prints = ToAvailableActionList("Print", databaseService.GetPrintList());
        var customProperties = ToAvailableActionList("Custom Properties", databaseService.GetCustomPropertiesList());
        var fileInfos = ToAvailableActionList("File Info", _databaseService.GetFileInfoList());
        var saveModels = ToAvailableActionList("Save Model", _databaseService.GetSaveModelList());
        var saveDrawings = ToAvailableActionList("Save Drawing", databaseService.GetSaveDrawingList());
        var importFiles = ToAvailableActionList("Import Files", _databaseService.GetImportFileList());
        var drawingTemplates = ToAvailableActionList("Drawing Templates", _databaseService.GetDrawingTemplateList());
        var macros = ToAvailableActionList("Macros", _databaseService.GetMacrosList());

        AvailableActionsList = new ObservableCollection<AvailableActionItemViewModel>(
            prints
                .Concat(customProperties)
                .Concat(fileInfos)
                .Concat(saveModels)
                .Concat(saveDrawings)
                .Concat(importFiles)
                .Concat(drawingTemplates)
                .Concat(macros)
        );

        ProcessList.FetchList();
    }

    // Design-time only
    public ProcessPageViewModel() : this(new MainViewModel(), new DialogService(() => null),
        new DatabaseService(new ApplicationDbContext()))
    {
        if (!Design.IsDesignMode)
            throw new InvalidOperationException("Parameterless constructor is only for design time use");
    }

    protected override void OnDesignTimeConstructor()
    {
        Initialize(new MainViewModel(), new DialogService(() => null), new DatabaseService(new ApplicationDbContext()));
    }

    #endregion
}