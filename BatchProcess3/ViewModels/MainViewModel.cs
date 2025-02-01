using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SomeWidth))]
    private bool _sideMenuExpanded;

    public int SomeWidth => SideMenuExpanded ? 220 : 75;

    /*public SvgImage SideMenuImage => new()
    {
        Source = SvgSource.Load(
            $"avares://{nameof(BatchProcess3)}/Assets/Images/{(SideMenuExpanded ? "logo" : "icon")}.svg")
    };*/

    [RelayCommand]
    private void SideMenuResize()
    {
        SideMenuExpanded = !SideMenuExpanded;
    }
}