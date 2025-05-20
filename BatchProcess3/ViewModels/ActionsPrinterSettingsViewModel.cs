using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ActionsPrinterSettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private KeyValuePair<string, string> _drawingColor = new("0", "(Default)");

    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, string>> _drawingColorOptions =
    [
        new("0", "(Default)"),
        new("1", "Automatic"),
        new("2", "Color / Greyscale"),
        new("3", "Black & White")
    ];

    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private string _id = "-1";

    [ObservableProperty]
    private KeyValuePair<string, string> _orientation = new("0", "(Default)");

    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, string>> _orientationOptions =
    [
        new("0", "(Default)"),
        new("1", "Portrait"),
        new("2", "Landscape")
    ];

    [ObservableProperty]
    private KeyValuePair<string, string> _printerName = new("0", "(Default)");

    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, string>> _printerNameOptions =
    [
        new("0", "(Default)")
    ];

    [ObservableProperty]
    private KeyValuePair<string, string> _printerSize = new("0", "(Default)");

    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, string>> _printerSizeOptions =
    [
        new("0", "(Default)")
    ];

    [ObservableProperty]
    private bool _scaleToFit;

    [ObservableProperty]
    private KeyValuePair<string, string> _sourceTray = new("0", "(Default)");

    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, string>> _sourceTrayOptions =
    [
        new("0", "(Default)")
    ];

    [ObservableProperty]
    private string _type = "A Size";

    [ObservableProperty]
    private double _width;
}