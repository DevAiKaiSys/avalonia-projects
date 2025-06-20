using System.Collections.Generic;
using AvaloniaLoudnessMeter.DataModels;

namespace AvaloniaLoudnessMeter.ViewModels;

public class ChannelGroupViewModel
{
    public ChannelGroupViewModel(string key, IEnumerable<ChannelConfigurationItem> items)
    {
        Key = key;
        Items = items;
    }

    public string Key { get; }
    public IEnumerable<ChannelConfigurationItem> Items { get; }
}