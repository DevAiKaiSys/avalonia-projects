using System.Text.Json;
using System.Text.Json.Serialization;
using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ActionsTabPrintViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _description = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DrawingExclusionListTitle))]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _drawingExclusionIsWhiteList;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _drawingExclusionList = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty]
    private bool _isNewItem;

    [ObservableProperty]
    [property: JsonIgnore]
    private bool _isSelected;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _printDrawingRange = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _printDrawings;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    /*private ActionsPrinterProfileViewModel _printerProfile = new();*/
    private string? _printerSettingsId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _printModels;

    public string DrawingExclusionListTitle => DrawingExclusionIsWhiteList ? "White List" : "Black List";

    [JsonIgnore]
    public new bool HasChanged =>
        IsNewItem || (SavedState != "" && SavedState != JsonSerializer.Serialize(this, _jsonOptions));

    public ActionsTabPrintDataModel ToDataModel()
    {
        return new ActionsTabPrintDataModel
        {
            Id = Id,
            Description = Description,
            DrawingExclusionIsWhiteList = DrawingExclusionIsWhiteList,
            DrawingExclusionList = DrawingExclusionList,
            JobName = JobName,
            PrintDrawingRange = PrintDrawingRange,
            PrintDrawings = PrintDrawings,
            PrinterSettingsId = PrinterSettingsId,
            PrintModels = PrintModels
        };
    }
}