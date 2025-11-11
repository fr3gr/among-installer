using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AmongUsModInstaller;

public class Updater
{
    private const string REPO_OWNER = "fr3gr";
    private const string REPO_NAME = "among-installer";
    private const string VERSION_FILE = "version.txt";
    
    public static string GetCurrentVersion()
    {
        try
        {
            var versionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, VERSION_FILE);
            if (File.Exists(versionPath))
                return File.ReadAllText(versionPath).Trim();
        }
        catch { }
        return "1.0.0";
    }
    
    public static async Task<(bool hasUpdate, string latestVersion, string downloadUrl)?> CheckForUpdates()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "AmongUsModInstaller");
            
            var response = await client.GetStringAsync(
                $"https://api.github.com/repos/{REPO_OWNER}/{REPO_NAME}/releases/latest");
            
            var json = JsonDocument.Parse(response);
            var root = json.RootElement;
            
            var latestVersion = root.GetProperty("tag_name").GetString()?.TrimStart('v') ?? "1.0.0";
            var currentVersion = GetCurrentVersion();
            
            if (CompareVersions(latestVersion, currentVersion) > 0)
            {
                var assets = root.GetProperty("assets");
                foreach (var asset in assets.EnumerateArray())
                {
                    var name = asset.GetProperty("name").GetString();
                    if (name != null && name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        var downloadUrl = asset.GetProperty("browser_download_url").GetString();
                        if (downloadUrl != null)
                            return (true, latestVersion, downloadUrl);
                    }
                }
            }
            
            return (false, latestVersion, "");
        }
        catch
        {
            return null;
        }
    }
    
    private static int CompareVersions(string v1, string v2)
    {
        var parts1 = v1.Split('.');
        var parts2 = v2.Split('.');
        var length = Math.Max(parts1.Length, parts2.Length);
        
        for (int i = 0; i < length; i++)
        {
            var num1 = i < parts1.Length && int.TryParse(parts1[i], out var n1) ? n1 : 0;
            var num2 = i < parts2.Length && int.TryParse(parts2[i], out var n2) ? n2 : 0;
            
            if (num1 != num2)
                return num1.CompareTo(num2);
        }
        
        return 0;
    }
    
    public static async Task<bool> DownloadAndInstallUpdate(string downloadUrl, Action<string> updateStatus)
    {
        try
        {
            var tempZip = Path.Combine(Path.GetTempPath(), "among_installer_update.zip");
            var tempDir = Path.Combine(Path.GetTempPath(), "among_installer_update");
            
            updateStatus("Pobieranie aktualizacji...");
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "AmongUsModInstaller");
                var data = await client.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(tempZip, data);
            }
            
            updateStatus("Rozpakowywanie...");
            
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);
            
            System.IO.Compression.ZipFile.ExtractToDirectory(tempZip, tempDir);
            
            var currentExe = Process.GetCurrentProcess().MainModule?.FileName ?? "";
            var currentDir = Path.GetDirectoryName(currentExe) ?? "";
            
            var batchPath = Path.Combine(Path.GetTempPath(), "update_installer.bat");
            var batchContent = $@"@echo off
timeout /t 2 /nobreak > nul
xcopy ""{tempDir}\*"" ""{currentDir}"" /E /Y /I
del ""{tempZip}""
rmdir /S /Q ""{tempDir}""
start """" ""{currentExe}""
del ""{batchPath}""
";
            
            await File.WriteAllTextAsync(batchPath, batchContent);
            
            Process.Start(new ProcessStartInfo
            {
                FileName = batchPath,
                CreateNoWindow = true,
                UseShellExecute = false
            });
            
            Application.Current.Shutdown();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
