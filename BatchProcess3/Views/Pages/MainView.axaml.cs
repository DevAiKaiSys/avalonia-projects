using Avalonia.Controls;
using Avalonia.Input;
using BatchProcess3.ViewModels.Pages;

namespace BatchProcess3.Views.Pages;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        if (DataContext is MainViewModel viewModel) viewModel.SideMenuResizeCommand.Execute(null);
    }
}