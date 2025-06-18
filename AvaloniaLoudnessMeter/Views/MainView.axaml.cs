using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

        // Gather the named controls
        _channelConfigButton = this.FindControl<Control>("ChannelConfigurationButton") ??
                               throw new Exception("Cannot find Channel Configuration Button by name");
        _channelConfigPopup = this.FindControl<Control>("ChannelConfigurationPopup") ??
                              throw new Exception("Cannot find Channel Configuration Popup by name");
        _mainGrid = this.FindControl<Control>("MainGrid") ?? throw new Exception("Cannot find Main Grid by name");
    }

    #endregion

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var position = _channelConfigButton.TranslatePoint(new Point(), _mainGrid) ??
                       throw new Exception("Cannot get TranslatePoint");

        Dispatcher.UIThread.Post(() =>
        {
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

    #endregion
}