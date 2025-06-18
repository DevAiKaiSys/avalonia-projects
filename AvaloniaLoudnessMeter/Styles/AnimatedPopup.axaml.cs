using System;
using System.Threading;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaLoudnessMeter.Styles;

public partial class AnimatedPopup : ContentControl
{
    #region Constructor

    /// <summary>
    ///     Default constructor
    /// </summary>
    public AnimatedPopup()
    {
        // Make a new dispatch timer
        _animationTimer = new DispatcherTimer
        {
            // Set the timer to run 60 times a second
            Interval = _frameRate
        };

        _sizingTimer = new Timer(_ =>
        {
            // If we have already calculated the size...
            if (_sizeFound)
                // No longer accept new sizes
                return;

            // We have now found our desired size
            _sizeFound = true;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                // Set the desired size
                _desiredSize = DesiredSize - Margin;

                // Update animation
                UpdateAnimation();
            });
        });

        // Callback on every tick
        _animationTimer.Tick += (_, _) => AnimationTick();
    }

    #endregion

    public override void Render(DrawingContext context)
    {
        // If we have not yet found the desired size...
        if (!_sizeFound)
        {
            // If we have not yet captured the opacity
            if (!_opacityCaptured)
            {
                // Set flag to true
                _opacityCaptured = true;

                // Remember original controls opacity
                _originalOpacity = Opacity;

                // Hide control
                Dispatcher.UIThread.Post(() => { Opacity = 0; });
            }

            _sizingTimer.Change(100, int.MaxValue);
        }

        base.Render(context);
    }

    #region Private Methods

    /// <summary>
    ///     Calculate and start any new required animations
    /// </summary>
    private void UpdateAnimation()
    {
        // Do nothing if we still haven't found our initial size
        if (!_sizeFound)
            return;

        // Start the animation thread again
        _animationTimer.Start();
    }

    /// <summary>
    ///     Update controls sizes based on the next tick of an animation
    /// </summary>
    private void AnimationTick()
    {
        // If this is the first call after calculating the desired size...
        if (_firstAnimation)
        {
            // Clear the flag
            _firstAnimation = false;

            // Stop this animation timer
            _animationTimer.Stop();

            // Reset opacity
            Opacity = _originalOpacity;

            // Set the final size
            Width = _open ? _desiredSize.Width : 0;
            Height = _open ? _desiredSize.Height : 0;

            // Do on this tick
            return;
        }

        // If we have reached the end of our animation...
        if ((_open && _animationCurrentTick >= TotalTicks) ||
            (!_open && _animationCurrentTick == 0))
        {
            // Stop this animation timer
            _animationTimer.Stop();

            // Set the final size
            Width = _open ? _desiredSize.Width : 0;
            Height = _open ? _desiredSize.Height : 0;

            // Break out of code
            return;
        }

        // Move the tick in the right direction
        _animationCurrentTick += _open ? 1 : -1;

        // Get percentage of the way through the current animation
        var percentageAnimated = (float)_animationCurrentTick / TotalTicks;

        // Make an animation easing
        var easing = new QuadraticEaseIn();

        // Calculate final width and height
        var finalWidth = _desiredSize.Width * easing.Ease(percentageAnimated);
        var finalHeight = _desiredSize.Height * easing.Ease(percentageAnimated);

        // Do our animation
        Width = finalWidth;
        Height = finalHeight;

        Console.WriteLine($"Current tick: {_animationCurrentTick}");
    }

    #endregion

    #region Private Members

    /// <summary>
    ///     Indicates if this is the first time we are animating
    /// </summary>
    private bool _firstAnimation = true;

    /// <summary>
    ///     Indicates if we have captured the opacity value yet
    /// </summary>
    private bool _opacityCaptured;

    /// <summary>
    ///     Store the controls original Opacity value at startup
    /// </summary>
    private double _originalOpacity;

    /// <summary>
    ///     The speed of the animation in FPS
    /// </summary>
    private readonly TimeSpan _frameRate = TimeSpan.FromSeconds(1 / 60.0);

    // Calculate total ticks that make up the animation time
    private int TotalTicks => (int)(_animationTime.TotalSeconds / _frameRate.TotalSeconds);

    /// <summary>
    ///     Store the controls desired size
    /// </summary>
    private Size _desiredSize;

    /// <summary>
    ///     Keeps track of if we have found the desired 100% width/height auto size
    /// </summary>
    private bool _sizeFound;

    /// <summary>
    ///     The animation UI timer
    /// </summary>
    private readonly DispatcherTimer _animationTimer;

    /// <summary>
    ///     The timeout timer to detect when auto-sizing has finished firing
    /// </summary>
    private readonly Timer _sizingTimer;

    /// <summary>
    ///     The current position in the animation
    /// </summary>
    private int _animationCurrentTick;

    #endregion

    #region Public Properties

    /// <summary>
    ///     Indicates if the control is currently opened
    /// </summary>
    public bool IsOpened => _animationCurrentTick >= TotalTicks;

    #region Open

    private bool _open;

    public static readonly DirectProperty<AnimatedPopup, bool> OpenProperty =
        AvaloniaProperty.RegisterDirect<AnimatedPopup, bool>(
            nameof(Open), o => o.Open, (o, v) => o.Open = v);

    /// <summary>
    ///     Property to set whether the control should be open or closed
    /// </summary>
    public bool Open
    {
        get => _open;
        set => SetAndRaise(OpenProperty, ref _open, value);
    }

    #endregion

    #region Animation Time

    private TimeSpan _animationTime = TimeSpan.FromSeconds(3);

    public static readonly DirectProperty<AnimatedPopup, TimeSpan> AnimationTimeProperty =
        AvaloniaProperty.RegisterDirect<AnimatedPopup, TimeSpan>(
            nameof(AnimationTime), o => o.AnimationTime, (o, v) => o.AnimationTime = v);

    public TimeSpan AnimationTime
    {
        get => _animationTime;
        set => SetAndRaise(AnimationTimeProperty, ref _animationTime, value);
    }

    #endregion

    #endregion

    #region Public Commands

    [RelayCommand]
    public void BeginOpen()
    {
        Open = true;

        // Update animation
        UpdateAnimation();
    }

    [RelayCommand]
    public void BeginClose()
    {
        Open = false;

        // Update animation
        UpdateAnimation();
    }

    #endregion
}