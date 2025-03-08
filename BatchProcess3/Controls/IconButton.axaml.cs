using Avalonia;
using Avalonia.Controls;

namespace BatchProcess3.Controls;

public class IconButton : Button
{
    /* short key styledProperty */
    public readonly static StyledProperty<string> IconTextProperty = AvaloniaProperty.Register<IconButton, string>(
        nameof(IconText));

    public string IconText
    {
        get => GetValue(IconTextProperty);
        set => SetValue(IconTextProperty, value);
    }
}