using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BatchProcess3.Core.SolidWorks;

namespace BatchProcess3.SolidWorks;

public class BatchProcessClient
{
    private readonly string _dummyDataPath;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private string _hostAddress = "";

    public BatchProcessClient()
    {
        // แก้ไขเรื่อง Path: พยายามหาโฟลเดอร์ SampleFiles โดยเริ่มจากโฟลเดอร์ที่รันโปรแกรม
        _dummyDataPath = ResolveDummyDataPath();
    }

    public bool DummyData { get; set; } = true;

    public void Connect(string hostAddress)
    {
        _hostAddress = hostAddress;
    }

    public async Task<List<SolidWorksFileDetails>> GetActiveFileReferencesAsync()
    {
        if (DummyData)
        {
            if (!Directory.Exists(_dummyDataPath))
            {
                Debug.WriteLine($"[ERROR] Directory not found: {_dummyDataPath}");
            }
            else
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
        }

        try
        {
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

    // Helper: ช่วยหา Path ให้ฉลาดขึ้น ไม่ว่าจะรันจาก Debugger หรือโฟลเดอร์อื่น
    private string ResolveDummyDataPath()
    {
        // 1. เริ่มจากโฟลเดอร์ที่รันโปรแกรมอยู่ (รองรับทั้ง / และ \ อัตโนมัติ)
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        // 2. วนลูปขึ้นไปหา Solution Root (สูงสุดไม่เกิน 6 ชั้นเพื่อประสิทธิภาพ)
        var maxDepth = 6;
        while (directory != null && maxDepth > 0)
        {
            // ใช้ EnumerateFiles เพื่อความเร็ว (หยุดทันทีที่เจอ .sln)
            if (directory.EnumerateFiles("*.sln").Any()) break;
            directory = directory.Parent;
            maxDepth--;
        }

        if (directory != null)
            // 3. ใช้ Path.Combine แทนการเขียน String ต่อกันเอง 
            // วิธีนี้ .NET จะเลือกใช้ / หรือ \ ให้ตามระบบปฏิบัติการที่รันอยู่
            return Path.Combine(directory.FullName, "BatchProcess3", "SolidWorks", "SampleFiles");

        // Fallback: หากหาไม่เจอจริงๆ ให้ใช้ Path ปัจจุบัน
        return AppDomain.CurrentDomain.BaseDirectory;
    }
}