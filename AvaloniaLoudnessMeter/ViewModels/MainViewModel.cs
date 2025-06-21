using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaLoudnessMeter.DataModels;
using AvaloniaLoudnessMeter.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaLoudnessMeter.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    #region Private Members

    private readonly IAudioInterfaceService _audioInterfaceService;

    #endregion

    #region Public Properties

    [ObservableProperty]
    private string _boldTitle = "AVALONIA";

    [ObservableProperty]
    private string _regularTitle = "LOUDNESS METER";

    [ObservableProperty]
    private bool _channelConfigurationListIsOpen;

    [ObservableProperty]
    private double _volumePercentPosition;

    [ObservableProperty]
    private double _volumeContainerSize;

    [ObservableProperty]
    /*private ObservableGroupedCollection<string, ChannelConfigurationItem> _channelConfigurations = default!;*/
    private ObservableCollection<ChannelGroupViewModel> _channelConfigurations = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ChannelConfigurationButtonText))]
    private ChannelConfigurationItem? _selectedChannelConfiguration;

    public string ChannelConfigurationButtonText => SelectedChannelConfiguration?.ShortText ?? "Select Channel";

    #endregion

    #region Public Commands

    [RelayCommand]
    private void ChannelConfigurationButtonPressed()
    {
        ChannelConfigurationListIsOpen ^= true;
    }

    [RelayCommand]
    private void ChannelConfigurationItemPressed(ChannelConfigurationItem item)
    {
        // Update the selected item
        SelectedChannelConfiguration = item;

        // Close the menu
        ChannelConfigurationListIsOpen = false;
    }

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        // Get the channel configuration data
        var channelConfigurations = await _audioInterfaceService.GetChannelConfigurationsAsync();

        // Create a grouping from the flat data
        var grouped = channelConfigurations
            .GroupBy(item => item.Group)
            .Select(g => new ChannelGroupViewModel(g.Key, g));

        foreach (var group in grouped)
            ChannelConfigurations.Add(group);
    }

    #endregion

    #region Constructor

    /// <summary>
    ///     Default constructor
    /// </summary>
    /// <param name="audioInterfaceService">The audio interface service</param>
    public MainViewModel(IAudioInterfaceService audioInterfaceService)
    {
        _audioInterfaceService = audioInterfaceService;
    }

    /// <summary>
    ///     Design-time constructor
    /// </summary>
    public MainViewModel()
    {
        _audioInterfaceService = new DummyAudioInterfaceService();

        Initialize();
    }

    private void Initialize()
    {
        // Temp code to move volume position

        var tick = 0;
        double input;

        var tempTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1 / 60.0)
        };

        tempTimer.Tick += (_, _) =>
        {
            tick++;

            // Slow down ticks
            input = tick / 20f;

            // Scale value
            var scale = VolumeContainerSize / 2f;

            VolumePercentPosition = (Math.Sin(input) + 1) * scale;
        };

        tempTimer.Start();

        #endregion
    }
}