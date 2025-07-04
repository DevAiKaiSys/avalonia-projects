using Avalonia.Controls;
using BatchProcess3.ViewModels;

namespace BatchProcess3.Views;

public partial class ActionsTabPrintView : UserControl
{
    public ActionsTabPrintView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is ActionsTabPrintViewModel viewModel)
        {
            // When a print view is selected
            /*viewModel.SetSavedState();*/

            // When it is a newly created item
            if (viewModel.IsNewItem)
            {
                JobNameTextBox.SelectAll();
                JobNameTextBox.Focus();
            }
        }
    }
}