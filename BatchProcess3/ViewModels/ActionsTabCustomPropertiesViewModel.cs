using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using BatchProcess3.CustomProperties;
using BatchProcess3.DataStorage.DataModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public partial class ActionsTabCustomPropertiesViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _changeNameTo;

    [ObservableProperty]
    private string _copyFromConfiguration;

    [ObservableProperty]
    private string _copyToField;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _description = "";

    [ObservableProperty]
    private bool _excludeAssemblies;

    [ObservableProperty]
    private bool _excludeDrawings;

    [ObservableProperty]
    private bool _excludeParts;

    [ObservableProperty]
    private string _fieldName;

    [ObservableProperty]
    private string _fieldType;

    [ObservableProperty]
    private ObservableCollection<string> _fieldTypeOptions = [];

    [ObservableProperty]
    private string _filterLogic;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _id = "";

    [ObservableProperty]
    private bool _isNewItem;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasChanged))]
    private string _jobName = "";

    [ObservableProperty]
    private CustomPropertyRuleType _ruleType;

    [ObservableProperty]
    private bool _setAllConfigSpecificProperties;

    [ObservableProperty]
    private bool _setCustomProperty;

    [ObservableProperty]
    private string _setNamedConfigurationProperties;

    [ObservableProperty]
    private string _valueRule;

    [JsonIgnore]
    public new bool HasChanged =>
        IsNewItem || (SavedState != "" && SavedState != JsonSerializer.Serialize(this, _jsonOptions));

    public ActionsTabCustomPropertiesDataModel ToDataModel()
    {
        return new ActionsTabCustomPropertiesDataModel
        {
            Id = Id,
            Description = Description,
            JobName = JobName,
            ChangeNameTo = ChangeNameTo,
            CopyFromConfiguration = CopyFromConfiguration,
            CopyToField = CopyToField,
            ExcludeAssemblies = ExcludeAssemblies,
            ExcludeDrawings = ExcludeDrawings,
            ExcludeParts = ExcludeParts,
            FieldName = FieldName,
            FieldType = FieldType,
            FilterLogic = FilterLogic,
            RuleType = RuleType,
            SetAllConfigSpecificProperties = SetAllConfigSpecificProperties,
            SetCustomProperty = SetCustomProperty,
            SetNamedConfigurationProperties = SetNamedConfigurationProperties,
            ValueRule = ValueRule
        };
    }
}