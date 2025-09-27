using System.Collections.Generic;

namespace BatchProcess3.DataStorage.DataModels;

public class ActionPrintSettingsDataModel : ActionDataModel
{
    public bool CanEdit { get; init; }

    public bool CanDelete { get; init; }

    public List<ActionPrintSettingsProfileDataModel> PrinterSettingProfiles { get; init; } = [];

    public List<ActionPrintDataModel> ActionsTabPrintDataModels { get; init; } = [];

    public int Copies { get; init; }
}