using Avalonia.Controls;
using Avalonia.Input;
using BatchProcess3.ViewModels;

namespace BatchProcess3;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();

        WindowState = WindowState.Maximized;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        if (DataContext is MainViewModel viewModel) viewModel.SideMenuResizeCommand.Execute(null);
    }
}