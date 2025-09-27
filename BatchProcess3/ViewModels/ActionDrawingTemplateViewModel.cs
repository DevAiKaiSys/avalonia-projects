using BatchProcess3.DataStorage.DataModels;
using BatchProcess3.DrawingTemplates;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ActionDrawingTemplateViewModel : ActionViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string? _currentTemplatePath;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string? _newTemplatePath;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    [NotifyPropertyChangedFor(nameof(CurrentTemplatePathIsVisible))]
    [NotifyPropertyChangedFor(nameof(NewTemplatePathIsVisible))]
    private DrawingTemplateOperation _operation;

    public bool CurrentTemplatePathIsVisible => Operation is DrawingTemplateOperation.Replace;

    public bool NewTemplatePathIsVisible => Operation is not DrawingTemplateOperation.Reload;

    public new ActionDrawingTemplateDataModel ToDataModel()
    {
        return new ActionDrawingTemplateDataModel
        {
            Id = Id,
            Description = Description,
            JobName = JobName,
            Operation = Operation,
            CurrentTemplatePath = CurrentTemplatePath,
            NewTemplatePath = NewTemplatePath
        };
    }
}

public static class ActionDrawingTemplateViewModelExtensions
{
    public static ActionDrawingTemplateViewModel ToViewModel(this ActionDrawingTemplateDataModel dataModel)
    {
        return new ActionDrawingTemplateViewModel
        {
            Id = dataModel.Id,
            JobName = dataModel.JobName,
            Description = dataModel.Description,
            CurrentTemplatePath = dataModel.CurrentTemplatePath,
            NewTemplatePath = dataModel.NewTemplatePath,
            Operation = dataModel.Operation
        };
    }
}