using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BatchProcess3.Core.SolidWorks;

namespace BatchProcess3.SolidWorks;

public class BatchProcessClient
{
    private readonly string _dummyDataPath =
        Path.Combine("..", "..", "..", "..", "BatchProcess3", "SolidWorks", "SampleFiles");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private string _hostAddress = "";

    public bool DummyData { get; set; } = true;

    public void Connect(string hostAddress)
    {
        _hostAddress = hostAddress;
    }

    public async Task<List<SolidWorksFileDetails>> GetActiveFileReferencesAsync()
    {
        try
        {
            if (DummyData)
            {
                var files = Directory.GetFiles(_dummyDataPath)
                    .Select(Path.GetFullPath)
                    .Where(f => !Path.GetFileName(f).StartsWith("~$"))
                    .Where(f => f.EndsWith(".sldprt", StringComparison.InvariantCultureIgnoreCase)
                                || f.EndsWith(".sldasm", StringComparison.InvariantCultureIgnoreCase)
                                || f.EndsWith(".slddrw", StringComparison.InvariantCultureIgnoreCase))
                    .Select(f => new SolidWorksFileDetails(f)).ToList();

                // By default, make the first *.sldasm the active file
                files.FirstOrDefault(f => f.FileName.EndsWith(".sldasm", StringComparison.InvariantCultureIgnoreCase))
                    ?.IsActiveInSolidWorks = true;

                // Return files
                return files;
            }

            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(_hostAddress + BatchProcessHostUrls.SolidWorksActiveFileList);

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<SolidWorksFileDetails>>(responseString, _jsonOptions) ?? [];

            return result;
        }
        catch (Exception)
        {
            // TODO: Handle somewhere
            return [];
        }
    }
}