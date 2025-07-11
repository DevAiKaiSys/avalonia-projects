using System;
using System.Collections.Generic;

namespace BatchProcess3.DataModels;

public class PrintSettingsDataModel
{
    public int Copies;

    public string Description = "";

    public string Name = "";

    public List<PrintSettingsProfileDataModel> PrinterSettings = [];
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
}