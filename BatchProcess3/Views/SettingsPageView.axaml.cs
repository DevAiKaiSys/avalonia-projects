using Avalonia.Controls;
using Avalonia.Interactivity;
using BatchProcess3.ViewModels;

namespace BatchProcess3.Views;

public partial class SettingsPageView : UserControl
{
    public SettingsPageView()
    {
        InitializeComponent();
    }

    /*protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        ((ViewModelBase)DataContext!).OnViewLoaded();
    }*/
    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        ((ViewModelBase)DataContext!).OnViewLoaded();
    }
}