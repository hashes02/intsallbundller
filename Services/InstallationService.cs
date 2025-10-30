using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AppBundle.Avalonia.ViewModels;

namespace AppBundle.Avalonia.Services;

/// <summary>
/// Handles application download and installation
/// </summary>
public class InstallationService
{
    private static readonly HttpClient httpClient = new HttpClient();

    /// <summary>
    /// Downloads and installs an application
    /// </summary>
    public async Task InstallAppAsync(AppItemViewModel appViewModel, IProgress<string> progress)
    {
        var tempFilePath = string.Empty;
        
        try
        {
            progress.Report("🔄 Resolving download URL...");
            appViewModel.Status = InstallStatus.Downloading;
            appViewModel.StatusText = "🔄 Resolving download URL...";

            var app = CreateAppInfoFromViewModel(appViewModel);
            var resolvedApp = await AppResolver.ResolveAppAsync(app);
            
            if (resolvedApp == null)
            {
                throw new Exception("Failed to resolve download URL");
            }

            progress.Report("🔄 Downloading...");
            appViewModel.StatusText = "🔄 Downloading...";

            tempFilePath = await DownloadFileAsync(resolvedApp.Url);

            // Verify hash if provided
            if (!string.IsNullOrWhiteSpace(resolvedApp.Sha256))
            {
                progress.Report("🔍 Verifying file integrity...");
                appViewModel.StatusText = "🔍 Verifying...";
                
                var isValid = await AppResolver.VerifySha256Async(tempFilePath, resolvedApp.Sha256);
                if (!isValid)
                {
                    throw new Exception("File integrity check failed");
                }
            }

            progress.Report("⚙️ Installing...");
            appViewModel.Status = InstallStatus.Installing;
            appViewModel.StatusText = "⚙️ Installing...";

            await InstallApplicationAsync(tempFilePath, app.Args ?? "/S");

            progress.Report("✅ Installed successfully");
            appViewModel.Status = InstallStatus.Done;
            appViewModel.StatusText = "✅ Installed successfully";
        }
        catch (Exception ex)
        {
            progress.Report($"❌ Failed: {ex.Message}");
            appViewModel.Status = InstallStatus.Failed;
            appViewModel.StatusText = $"❌ Failed: {ex.Message}";
        }
        finally
        {
            // Clean up temp file
            if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
            {
                try
                {
                    File.Delete(tempFilePath);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }
    }

    private AppInfo CreateAppInfoFromViewModel(AppItemViewModel appViewModel)
    {
        return new AppInfo
        {
            Id = appViewModel.Id,
            Name = appViewModel.Name,
            Source = GetAppSource(appViewModel.Id),
            Args = GetAppArgs(appViewModel.Id),
            Detect = GetAppDetect(appViewModel.Id)
        };
    }

    private async Task<string> DownloadFileAsync(string url)
    {
        var tempPath = Path.GetTempPath();
        var fileName = $"AppBundle_{Guid.NewGuid()}_{Path.GetFileName(new Uri(url).LocalPath)}";
        var filePath = Path.Combine(tempPath, fileName);

        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        await response.Content.CopyToAsync(fileStream);

        return filePath;
    }

    private async Task InstallApplicationAsync(string installerPath, string args)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = installerPath,
            Arguments = args,
            UseShellExecute = true,
            Verb = "runas",
            CreateNoWindow = true
        };

        using var process = Process.Start(processInfo);
        if (process == null)
        {
            throw new Exception("Failed to start installer process");
        }

        await process.WaitForExitAsync();

        // 1641 and 3010 are Windows Installer success codes indicating restart required
        if (process.ExitCode != 0 && process.ExitCode != 1641 && process.ExitCode != 3010)
        {
            throw new Exception($"Installation failed with exit code {process.ExitCode}");
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