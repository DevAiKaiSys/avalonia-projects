using Avalonia.Controls;
using ActionSaveModelViewModel = BatchProcess3.ViewModels.Actions.ActionSaveModelViewModel;

namespace BatchProcess3.Views.Actions;

public partial class ActionSaveModelView : UserControl
{
    public ActionSaveModelView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems?.Count > 0 && e.AddedItems[0] is ActionSaveModelViewModel viewModel)
            // When it is a newly created item
            if (viewModel.IsNewItem)
            {
                JobNameTextBox.SelectAll();
                JobNameTextBox.Focus();
            }
    }
}