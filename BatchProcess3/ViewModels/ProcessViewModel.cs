using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ProcessViewModel : ViewModelBase, ISelectableItemListViewModel
{
    private ObservableCollection<ProcessActionViewModel> _actions = [];

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _description = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty] private bool _isNewItem;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public ProcessViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        Actions = [];
    }

    [JsonIgnore]
    public override bool HasChanged =>
        IsNewItem || (SavedState != "" && SavedState != JsonSerializer.Serialize(this, GetType(), _jsonOptions));

    public ObservableCollection<ProcessActionViewModel> Actions
    {
        get => _actions;
        set => this.SetAndObserveEverything(value, ref _actions, [nameof(HasChanged)]);
    }

    public ProcessDataModel ToDataModel()
    {
        return new ProcessDataModel
        {
            Id = Id,
            Description = Description,
            JobName = JobName,
            Actions = Actions.Select(f => f.ToDataModel()).ToList()
        };
    }

    public override string ToString()
    {
        return $"{JobName} ({Description})";
    }
}

public static class ProcessViewModelExtensions
{
    public static ProcessViewModel ToViewModel(this ProcessDataModel dataModel)
    {
        return new ProcessViewModel
        {
            Id = dataModel.Id,
            JobName = dataModel.JobName,
            Description = dataModel.Description,
            Actions = new ObservableCollection<ProcessActionViewModel>(dataModel.Actions
                .Select(f => f.ToViewModel())
                .OrderBy(f => f.SortOrder))
        };
    }
}