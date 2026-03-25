using System.Collections.ObjectModel;
using BatchProcess3.Core.SolidWorks;
using BatchProcess3.ViewModels.Actions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class JobViewModel : ViewModelBase
{
    #region Properties

    [ObservableProperty] private ObservableCollection<SolidWorksFileDetails> _solidWorksFileList = [];

    [ObservableProperty] private ObservableCollection<ProcessActionViewModel> _actions = [];

    [ObservableProperty] private bool _quickView;

    [ObservableProperty] private bool _saveOnClose = true;

    #endregion Properties
}