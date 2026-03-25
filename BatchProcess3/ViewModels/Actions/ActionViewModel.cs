using System.Text.Json;
using System.Text.Json.Serialization;
using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels.Actions;

public partial class ActionViewModel : ViewModelBase
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _description = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty] private bool _isNewItem;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    [NotifyPropertyChangedFor(nameof(SortOrderDisplayString))]
    private int _sortOrder;

    public string SortOrderDisplayString => $"{SortOrder + 1:00\\.}";

    [JsonIgnore]
    public override bool HasChanged =>
        IsNewItem || (SavedState != "" && SavedState != JsonSerializer.Serialize(this, GetType(), _jsonOptions));

    public ActionDataModel ToDataModel()
    {
        return new ActionDataModel
        {
            Id = Id,
            Description = Description,
            JobName = JobName,
            SortOrder = SortOrder
        };
    }
}

public static class ActionViewModelExtensions
{
    public static ActionViewModel ToViewModel(this ActionDataModel dataModel)
    {
        return new ActionViewModel
        {
            Id = dataModel.Id,
            JobName = dataModel.JobName,
            Description = dataModel.Description,
            SortOrder = dataModel.SortOrder
        };
    }
}