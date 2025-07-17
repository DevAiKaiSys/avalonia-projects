using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ErrorViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _description = "Unknown Error Description";

    [ObservableProperty]
    private string _title = "Unknown Error";
}