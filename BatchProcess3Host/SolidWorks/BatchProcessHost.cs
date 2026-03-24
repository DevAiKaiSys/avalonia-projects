using System.Collections.Generic;
using System.IO;
using System.Linq;
using BatchProcess3.Core.SolidWorks;

namespace BatchProcess3Host.SolidWorks;

public class BatchProcessHost
{
    public string GetSolidWorksVersion()
    {
        return "SolidWorks 2025 SP1.2";
    }

    public SolidWorksFileDetails GetActiveFile()
    {
        var filePath = Path.Combine("SolidWorks", "Test Files", "Assem1.SLDASM");
        return new SolidWorksFileDetails(filePath);
    }

    public List<SolidWorksFileDetails> GetActiveFileReferences()
    {
        var directoryPath = Path.Combine("SolidWorks", "Test Files");

        if (!Directory.Exists(directoryPath)) return new List<SolidWorksFileDetails>();

        return new List<SolidWorksFileDetails>(
            Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Select(f => new SolidWorksFileDetails(f)));
    }
}