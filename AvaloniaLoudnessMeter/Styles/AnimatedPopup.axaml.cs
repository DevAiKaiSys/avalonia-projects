using System;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace AvaloniaLoudnessMeter.Styles;

public class AnimatedPopup : ContentControl
{
    #region Constructor

    /// <summary>
    ///     Default constructor
    /// </summary>
    public AnimatedPopup()
    {
        // Get a 60 FPS timespan
        var framerate = TimeSpan.FromSeconds(1 / 60.0);

        // Make a new dispatch timer
        _animationTimer = new DispatcherTimer
        {
            // Set the timer to run 60 times a second
            Interval = framerate
        };

        // Fix for 1 seconds
        var animationTime = TimeSpan.FromSeconds(1);

        // Calculate total ticks that make up the animation time
        var totalTicks = (int)(animationTime.TotalSeconds / framerate.TotalSeconds);

        // Keep track of current tick
        _animationCurrentTick = 0;

        // Callback on every tick
        _animationTimer.Tick += (_, _) =>
        {
            // Increment the tick
            _animationCurrentTick++;

            // Set animating flag
            _animating = true;

            // If we have reached the total ticks...
            if (_animationCurrentTick > totalTicks)
            {
                // Stop this animation timer
                _animationTimer.Stop();

                // Clear animating flag
                _animating = false;

                // Break out of code
                return;
            }

            // Get percentage of the way through the current animation
            var percentageAnimated = (float)_animationCurrentTick / totalTicks;

            // Make an animation easing
            var easing = new QuadraticEaseIn();

            // Calculate final width and height
            var finalWidth = _desiredSize.Width * easing.Ease(percentageAnimated);
            var finalHeight = _desiredSize.Height * easing.Ease(percentageAnimated);

            // Do our animation
            Width = finalWidth;
            Height = finalHeight;

            Console.WriteLine($"Current tick: {_animationCurrentTick}");
        };
    }

    #endregion

    public override void Render(DrawingContext context)
    {
        // If we are not animating...
        if (!_animating)
        {
            // Set desired size (which includes margin, so remove that from our calculation)
            _desiredSize = DesiredSize - Margin;

            // Reset animation position
            _animationCurrentTick = 0;

            // Start timer
            _animationTimer.Start();

            Console.WriteLine($"Desired size: {_desiredSize}");
        }

        base.Render(context);
    }

    #region Private MembersAdd commentMore actions

    /// <summary>
    ///     Store the controls desired size
    /// </summary>
    private Size _desiredSize;

    /// <summary>
    ///     A flag for when we are animating
    /// </summary>
    private bool _animating;

    /// <summary>
    ///     The animation UI timer
    /// </summary>
    private readonly DispatcherTimer _animationTimer;

    /// <summary>
    ///     The current position in the animation
    /// </summary>
    private int _animationCurrentTick;

    #endregion
}