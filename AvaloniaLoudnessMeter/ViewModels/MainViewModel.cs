using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaLoudnessMeter.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _boldTitle = "AVALONIA";

    [ObservableProperty]
    private bool _channelConfigurationListIsOpen;

    [ObservableProperty]
    private string _regularTitle = "LOUDNESS METER";

    [RelayCommand]
    private void ChannelConfigurationButtonPressed()
    {
        ChannelConfigurationListIsOpen ^= true;
    }
}