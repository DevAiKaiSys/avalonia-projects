using Avalonia.Controls;
using BatchProcess3.ViewModels;

namespace BatchProcess3.Views;

public partial class ActionsPrintView : UserControl
{
    public ActionsPrintView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is ActionsPrintViewModel
            {
                IsNewItem: true
            } viewModel)
        {
            JobNameTextBox.SelectAll();
            JobNameTextBox.Focus();
        }
    }
}