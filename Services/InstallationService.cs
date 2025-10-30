using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AppBundle.Avalonia.ViewModels;

namespace AppBundle.Avalonia.Services;

public class InstallationService
{
    private readonly HttpClient httpClient = new();

    public async Task InstallAppAsync(AppItemViewModel appViewModel, IProgress<string> progress)
    {
        try
        {
            progress.Report("🔄 Resolving download URL...");
            appViewModel.Status = InstallStatus.Downloading;
            appViewModel.StatusText = "🔄 Resolving download URL...";

            // Get the app data
            var app = new AppInfo 
            { 
                Id = appViewModel.Id, 
                Name = appViewModel.Name, 
                Source = GetAppSource(appViewModel.Id),
                Args = GetAppArgs(appViewModel.Id),
                Detect = GetAppDetect(appViewModel.Id)
            };

            // Resolve the download URL
            var resolvedApp = await AppResolver.ResolveAppAsync(app);
            if (resolvedApp == null)
            {
                throw new Exception("Failed to resolve download URL");
            }

            progress.Report("🔄 Downloading...");
            appViewModel.StatusText = "🔄 Downloading...";

            // Download the installer
            var tempPath = Path.GetTempPath();
            var fileName = Path.GetFileName(new Uri(resolvedApp.Url).LocalPath);
            var filePath = Path.Combine(tempPath, fileName);

            using (var response = await httpClient.GetAsync(resolvedApp.Url))
            {
                response.EnsureSuccessStatusCode();
                using (var fileStream = File.Create(filePath))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }

            progress.Report("⚙️ Installing...");
            appViewModel.Status = InstallStatus.Installing;
            appViewModel.StatusText = "⚙️ Installing...";

            // Install the application
            var processInfo = new ProcessStartInfo
            {
                FileName = filePath,
                Arguments = app.Args ?? "",
                UseShellExecute = true,
                Verb = "runas", // Run as administrator
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                if (process != null)
                {
                    await process.WaitForExitAsync();
                    
                    if (process.ExitCode == 0)
                    {
                        progress.Report("✅ Installed successfully");
                        appViewModel.Status = InstallStatus.Done;
                        appViewModel.StatusText = "✅ Installed successfully";
                    }
                    else
                    {
                        throw new Exception($"Installation failed with exit code {process.ExitCode}");
                    }
                }
            }

            // Clean up temp file
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
        catch (Exception ex)
        {
            progress.Report($"❌ Failed: {ex.Message}");
            appViewModel.Status = InstallStatus.Failed;
            appViewModel.StatusText = $"❌ Failed: {ex.Message}";
        }
    }

    private string GetAppSource(string appId)
    {
        return appId switch
        {
            "chrome" => "omaha",
            "vlc" => "videolan",
            _ => "direct"
        };
    }

    private string GetAppArgs(string appId)
    {
        return appId switch
        {
            "chrome" => "/silent /install",
            "vlc" => "/S",
            "m365" => "/S",
            "7zip" => "/S",
            "rustdesk" => "/S",
            "zoom" => "/quiet /norestart",
            _ => "/S"
        };
    }

    private string GetAppDetect(string appId)
    {
        return appId switch
        {
            "chrome" => @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Google Chrome",
            "vlc" => @"HKLM\SOFTWARE\VideoLAN\VLC",
            "m365" => @"HKLM\SOFTWARE\Microsoft\Office\ClickToRun\Configuration",
            "7zip" => @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\7-Zip",
            "rustdesk" => @"HKLM\SOFTWARE\RustDesk",
            "zoom" => @"HKLM\SOFTWARE\Zoom\ZDC",
            _ => ""
        };
    }
}