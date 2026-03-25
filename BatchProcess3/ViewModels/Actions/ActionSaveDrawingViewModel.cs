using System.Collections.ObjectModel;
using System.Linq;
using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels.Actions;

public partial class ActionSaveDrawingViewModel : ActionViewModel, ISelectableItemListViewModel
{
    private ObservableCollection<KeyValueViewModel<string, bool>> _exportFormats;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _fileName = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _saveLocation = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _sheetsFilter = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _singleDwgDxf;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _singleEDrawing;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private bool _singlePdf;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public ActionSaveDrawingViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        ExportFormats = [];
    }

    public ObservableCollection<KeyValueViewModel<string, bool>> ExportFormats
    {
        get => _exportFormats;
        set => this.SetAndObserveEverything(value, ref _exportFormats, [nameof(HasChanged)]);
    }

    public new ActionSaveDrawingDataModel ToDataModel()
    {
        return new ActionSaveDrawingDataModel
        {
            Id = Id,
            Description = Description,
            JobName = JobName,
            FileName = FileName,
            SaveLocation = SaveLocation,
            ExportFormats = ExportFormats.Where(f => f.Value).Select(f => f.Key).ToList(),
            SheetsFilter = SheetsFilter,
            SingleDwgDxf = SingleDwgDxf,
            SingleEDrawing = SingleEDrawing,
            SinglePdf = SinglePdf
        };
    }
}

public static class ActionSaveDrawingViewModelExtensions
{
    public static ActionSaveDrawingViewModel ToViewModel(this ActionSaveDrawingDataModel dataModel,
        ObservableCollection<string> exportFormats)
    {
        return new ActionSaveDrawingViewModel
        {
            Id = dataModel.Id,
            JobName = dataModel.JobName,
            Description = dataModel.Description,
            SaveLocation = dataModel.SaveLocation,
            ExportFormats =
                new ObservableCollection<KeyValueViewModel<string, bool>>(exportFormats.Select(f =>
                    new KeyValueViewModel<string, bool>(f, dataModel.ExportFormats.Any(e => e == f)))),
            FileName = dataModel.FileName,
            SheetsFilter = dataModel.SheetsFilter,
            SingleDwgDxf = dataModel.SingleDwgDxf,
            SinglePdf = dataModel.SinglePdf,
            SingleEDrawing = dataModel.SingleEDrawing
        };
    }
}