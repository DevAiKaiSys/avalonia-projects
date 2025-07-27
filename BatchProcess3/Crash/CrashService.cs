using System;
using System.IO;
using System.Text.Json;

namespace BatchProcess3.Crash;

public class CrashService
{
    private static readonly string ParentFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        // Windows -> C:\ProgramData\BatchProcess3
        "BatchProcess3");

    private static readonly string CrashFilePath = Path.Combine(ParentFolder, "lastcrash.json");

    public static bool SetCrashData(Exception ex)
    {
        try
        {
            Directory.CreateDirectory(ParentFolder);

            File.WriteAllText(CrashFilePath, JsonSerializer.Serialize(new CrashData
            {
                CrashDate = DateTimeOffset.UtcNow,
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace ?? string.Empty,
                Source = ex.TargetSite?.ToString() ?? string.Empty
            }));

            return true;
        }
        catch (Exception)
        {
            // TODO: Handle system message box or other way to inform user of crash
        }

        return false;
    }

    public static void ClearCrashData()
    {
        try
        {
            if (File.Exists(CrashFilePath))
                File.Delete(CrashFilePath);
        }
        catch (Exception)
        {
            // Ignored    
        }
    }

    public static CrashData? GetCrashData()
    {
        try
        {
            if (File.Exists(CrashFilePath))
                return JsonSerializer.Deserialize<CrashData>(File.ReadAllText(CrashFilePath));
        }
        catch (Exception)
        {
            ClearCrashData();
        }

        return null;
    }
}