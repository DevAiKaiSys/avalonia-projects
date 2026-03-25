using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels.Actions;

public partial class ActionFileInfoViewModel : ActionViewModel, ISelectableItemListViewModel
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _author = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _comments = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _keywords = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _subject = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _title = "";

    public new ActionFileInfoDataModel ToDataModel()
    {
        return new ActionFileInfoDataModel
        {
            Id = Id,
            Description = Description,
            JobName = JobName,
            Author = Author,
            Comments = Comments,
            Keywords = Keywords,
            Subject = Subject,
            Title = Title
        };
    }
}

public static class ActionFileInfoViewModelExtensions
{
    public static ActionFileInfoViewModel ToViewModel(this ActionFileInfoDataModel dataModel)
    {
        return new ActionFileInfoViewModel
        {
            Id = dataModel.Id,
            Description = dataModel.Description,
            JobName = dataModel.JobName,
            Author = dataModel.Author,
            Comments = dataModel.Comments,
            Keywords = dataModel.Keywords,
            Subject = dataModel.Subject,
            Title = dataModel.Title
        };
    }
}