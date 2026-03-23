using System.Collections.ObjectModel;
using BatchProcess3.Core.SolidWorks;
using BatchProcess3Host.SolidWorks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BatchProcess3Host.ViewModels;

public partial class MainWindowViewModel(BatchProcessHost batchProcessHost) : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<SolidWorksFileDetails> _activeFilesList =
        new(batchProcessHost.GetActiveFileReferences());

    private int _count;

    [ObservableProperty] private string _greeting = "Welcome to Avalonia!";

    public MainWindowViewModel() : this(new BatchProcessHost())
    {
    }

    [RelayCommand]
    private void IncrementValue()
    {
        Greeting = $"New value: {_count++}";
    }
}