using Avalonia.Controls;
using ActionSaveDrawingViewModel = BatchProcess3.ViewModels.Actions.ActionSaveDrawingViewModel;

namespace BatchProcess3.Views.Actions;

public partial class ActionSaveDrawingView : UserControl
{
    public ActionSaveDrawingView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems?.Count > 0 && e.AddedItems[0] is ActionSaveDrawingViewModel viewModel)
            // When it is a newly created item
            if (viewModel.IsNewItem)
            {
                JobNameTextBox.SelectAll();
                JobNameTextBox.Focus();
            }
    }
}