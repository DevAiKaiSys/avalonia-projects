using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ActionPrintSettingsProfileViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _drawingColor = "(Default)";

    [ObservableProperty]
    private ObservableCollection<string> _drawingColorOptions =
    [
        "(Default)",
        "Automatic",
        "Color / Greyscale",
        "Black & White"
    ];

    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private string _id = "-1";

    [ObservableProperty]
    private string _orientation = "(Default)";

    [ObservableProperty]
    private ObservableCollection<string> _orientationOptions =
    [
        "(Default)",
        "Portrait",
        "Landscape"
    ];

    [ObservableProperty]
    private string _paperSize = "(Default)";

    [ObservableProperty]
    private ObservableCollection<string> _paperSizeOptions =
    [
        "(Default)"
    ];

    [ObservableProperty]
    private string _printerName = "(Default)";

    [ObservableProperty]
    private ObservableCollection<string> _printerNameOptions =
    [
        "(Default)"
    ];

    [ObservableProperty]
    private bool _scaleToFit;

    [ObservableProperty]
    private string _sourceTray = "(Default)";

    [ObservableProperty]
    private ObservableCollection<string> _sourceTrayOptions =
    [
        "(Default)"
    ];

    [ObservableProperty]
    private string _type = "A Size";

    [ObservableProperty]
    private double _width;
}

public static class ActionPrintSettingsProfileViewModelExtensions
{
    public static ActionPrintSettingsProfileDataModel ToDataModel(this ActionPrintSettingsProfileViewModel viewModel)
    {
        return new ActionPrintSettingsProfileDataModel
        {
            Id = viewModel.Id,
            Type = viewModel.Type,
            PrinterName = viewModel.PrinterName,
            DrawingColor = viewModel.DrawingColor,
            Height = viewModel.Height,
            Width = viewModel.Width,
            Orientation = viewModel.Orientation,
            SourceTray = viewModel.SourceTray,
            PaperSize = viewModel.PaperSize,
            ScaleToFit = viewModel.ScaleToFit
        };
    }

    public static List<ActionPrintSettingsProfileDataModel> ToDataModels(
        this ObservableCollection<ActionPrintSettingsProfileViewModel> viewModels)
    {
        return viewModels.Select(ToDataModel).ToList();
    }

    public static ActionPrintSettingsProfileViewModel ToViewModel(this ActionPrintSettingsProfileDataModel dataModel)
    {
        return new ActionPrintSettingsProfileViewModel
        {
            Id = dataModel.Id,
            Type = dataModel.Type,
            PrinterName = dataModel.PrinterName,
            DrawingColor = dataModel.DrawingColor,
            Height = dataModel.Height,
            Width = dataModel.Width,
            Orientation = dataModel.Orientation,
            SourceTray = dataModel.SourceTray,
            PaperSize = dataModel.PaperSize,
            ScaleToFit = dataModel.ScaleToFit
        };
    }

    public static ObservableCollection<ActionPrintSettingsProfileViewModel> ToViewModels(
        this List<ActionPrintSettingsProfileDataModel> dataModels)
    {
        return new ObservableCollection<ActionPrintSettingsProfileViewModel>(dataModels.Select(ToViewModel).ToList());
    }
}