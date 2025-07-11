using System;

namespace BatchProcess3.DataModels;

public class ActionsTabPrintDataModel
{
    public string Description = "";

    public bool DrawingExclusionIsWhiteList;

    public string DrawingExclusionList = "";

    public string JobName = "";

    public string PrintDrawingRange = "";

    public bool PrintDrawings;

    public string PrinterSettingsId = "";

    public bool PrintModels;
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    //public PrintSettingsDataModel PrinterSettings = "";
}