using Avalonia.Controls;
using ActionCustomPropertiesViewModel = BatchProcess3.ViewModels.Actions.ActionCustomPropertiesViewModel;

namespace BatchProcess3.Views.Actions;

public partial class ActionCustomPropertiesView : UserControl
{
    public ActionCustomPropertiesView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems?.Count > 0 && e.AddedItems[0] is ActionCustomPropertiesViewModel viewModel)
            // When it is a newly created item
            if (viewModel.IsNewItem)
            {
                JobNameTextBox.SelectAll();
                JobNameTextBox.Focus();
            }
    }
}