using System;

namespace BatchProcess3.DataModels;

public class PrintSettingsProfileDataModel
{
    public string DrawingColor = "";

    public double Height;

    public string Orientation = "";

    public string PaperSize = "";

    public string PrinterName = "";

    public bool ScaleToFit;

    public string SourceTray = "";

    public string Type = "";

    public double Width;
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string PrintSettingsDataModelId { get; set; } = "";

    public PrintSettingsDataModel PrintSettingsDataModel { get; set; } = new();
}