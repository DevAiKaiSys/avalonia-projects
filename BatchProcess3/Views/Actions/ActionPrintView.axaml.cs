using Avalonia.Controls;
using ActionPrintViewModel = BatchProcess3.ViewModels.Actions.ActionPrintViewModel;

namespace BatchProcess3.Views.Actions;

public partial class ActionPrintView : UserControl
{
    public ActionPrintView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems?.Count > 0 && e.AddedItems[0] is ActionPrintViewModel viewModel)
            // When it is a newly created item
            if (viewModel.IsNewItem)
            {
                JobNameTextBox.SelectAll();
                JobNameTextBox.Focus();
            }
    }
}