using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3.ViewModels;

public partial class ConfirmDialogViewModel : DialogViewModel
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    private bool _busy;

    [ObservableProperty]
    private string _cancelText = "No";

    [ObservableProperty]
    private bool _confirmed;

    [ObservableProperty]
    private string _confirmText = "Yes";

    [ObservableProperty]
    private double _dialogHeight = double.NaN;

    [ObservableProperty]
    private double _dialogWidth = double.NaN;

    [ObservableProperty]
    private string _iconText = "\xe4e0";

    [ObservableProperty]
    private string _message = "Are you sure?";

    [ObservableProperty]
    private string _progressText = "";

    [ObservableProperty]
    private string _statusText = "";

    [ObservableProperty]
    private string _title = "Confirm";

    public Func<ConfirmDialogViewModel, Task<bool>> OnConfirm { get; set; } = _ => Task.FromResult(true);

    public bool NotBusy()
    {
        return !Busy;
    }

    [RelayCommand]
    // public void Confirm()
    public async Task ConfirmAsync()
    {
        if (Busy)
            return;

        Busy = true;

        // Clear status text
        StatusText = "";

        // Set initial progress text
        ProgressText = "Processing...";

        // For test
        /*await Task.Delay(2000);
        ProgressText = "This is taking a while...";
        await Task.Delay(2000);*/


        var result = await OnConfirm(this);

        Busy = false;

        if (!result)
            return;

        Confirmed = true;
        Close();
    }

    /*[RelayCommand]
    public void Cancel()*/
    [RelayCommand(CanExecute = nameof(NotBusy))]
    public async void Cancel()
    {
        Confirmed = false;
        Close();
    }
}