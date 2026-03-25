using BatchProcess3.MainApp;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels.Pages;

/*public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty]
    private ApplicationPageNames _pageName;
}*/
/*public partial class PageViewModel(ApplicationPageNames pageName) : ViewModelBase
{
    [ObservableProperty]
    private ApplicationPageNames _pageName = pageName;
}*/
public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty] private ApplicationPageNames _pageName;

    protected PageViewModel(ApplicationPageNames pageName)
    {
        _pageName = pageName;
    }
}