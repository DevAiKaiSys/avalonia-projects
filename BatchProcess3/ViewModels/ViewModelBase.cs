using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BatchProcess3.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected readonly JsonSerializerOptions _jsonOptions = new()
    {
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        WriteIndented = true,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    [property: JsonIgnore]
    public string SavedState = "";

    public ViewModelBase()
    {
        // Detect design time
        if (Design.IsDesignMode)
            OnDesignTimeConstructor();
    }

    [JsonIgnore]
    public virtual bool HasChanged => SavedState != "" && SavedState != JsonSerializer.Serialize(this, _jsonOptions);

    protected virtual void OnDesignTimeConstructor()
    {
    }

    public virtual void OnViewLoaded()
    {
    }

    public void RaiseOnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }

    public void SetSavedState()
    {
        SavedState = GetState();

        OnPropertyChanged(nameof(HasChanged));
    }

    public string GetState()
    {
        return JsonSerializer.Serialize(this, GetType().DeclaringType ?? GetType(), _jsonOptions);
    }

    public void RestoreState(string? stateToRestore = null)
    {
        stateToRestore ??= SavedState;

        var type = GetType().DeclaringType ?? GetType();

        var savedState = JsonSerializer.Deserialize(stateToRestore, type, _jsonOptions);

        foreach (var propertyInfo in type.GetProperties())
        {
            // Only set setters, not get only properties
            if (!propertyInfo.CanWrite)
                continue;

            // Ignore any properties that have a JsonIgnore attribute
            if (propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).GetLength(0) > 0)
                continue;

            // Pull the saved value
            var originalValue = propertyInfo.GetValue(savedState);

            // Restore it to this class
            propertyInfo.SetValue(this, originalValue);
        }
    }
}