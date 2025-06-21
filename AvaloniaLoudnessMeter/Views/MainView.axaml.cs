using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaLoudnessMeter.ViewModels;

namespace AvaloniaLoudnessMeter.Views;

public partial class MainView : UserControl
{
    #region Constructor

    /// <summary>
    ///     Default constructor
    /// </summary>
    /// <exception cref="Exception">Throws if named controls cannot be found</exception>
    public MainView()
    {
        InitializeComponent();

        _sizingTimer = new Timer(_ =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                // Update the desired size
                UpdateSizes();
            });
        });

        // Gather the named controls
        _channelConfigButton = this.FindControl<Control>("ChannelConfigurationButton") ??
                               throw new Exception("Cannot find Channel Configuration Button by name");
        _channelConfigPopup = this.FindControl<Control>("ChannelConfigurationPopup") ??
                              throw new Exception("Cannot find Channel Configuration Popup by name");
        _mainGrid = this.FindControl<Control>("MainGrid") ?? throw new Exception("Cannot find Main Grid by name");
        _volumeContainer = this.FindControl<Control>("VolumeContainer") ??
                           throw new Exception("Cannot find Volume Container by name");
    }

    #endregion

    private void UpdateSizes()
    {
        ((MainViewModel)DataContext!).VolumeContainerSize = _volumeContainer.Bounds.Height;
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        await ((MainViewModel)DataContext!).LoadSettingsCommand.ExecuteAsync(null);

        base.OnLoaded(e);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        _sizingTimer.Change(100, int.MaxValue);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            // Get relative position of button, in relation to main grid
            var position = _channelConfigButton.TranslatePoint(new Point(), _mainGrid) ??
                           throw new Exception("Cannot get TranslatePoint from Configuration Button");

            // Set margin of popup so it appears bottom left of button
            _channelConfigPopup.Margin = new Thickness(
                position.X,
                0,
                0,
                _mainGrid.Bounds.Height - position.Y - _channelConfigButton.Bounds.Height);
        });
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        ((MainViewModel)DataContext!).ChannelConfigurationButtonPressedCommand.Execute(null);
    }


    #region Private Members

    private readonly Control _channelConfigPopup;
    private readonly Control _channelConfigButton;
    private readonly Control _mainGrid;
    private readonly Control _volumeContainer;


    /// <summary>
    ///     The timeout timer to detect when auto-sizing has finished firing
    /// </summary>
    private readonly Timer _sizingTimer;

    #endregion
}