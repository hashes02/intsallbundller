using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AppBundle.Avalonia.Services;

namespace AppBundle.Avalonia.ViewModels;

/// <summary>
/// AetherInstall wizard-based main window view model with multiple pages
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly InstallationService installationService = new();

    [ObservableProperty]
    private int currentPage = 0;

    [ObservableProperty]
    private int overallProgress = 0;

    [ObservableProperty]
    private bool isIndeterminate = false;

    [ObservableProperty]
    private string statusText = "Preparing installation...";

    [ObservableProperty]
    private string successMessage = "";

    public ObservableCollection<AppItemViewModel> Apps { get; } = new();
    public ObservableCollection<string> InstallLogs { get; } = new();

    public int SelectedCount => Apps.Count(a => a.IsSelected);
    public bool HasSelections => SelectedCount > 0;
    
    // Navigation properties for AetherInstall UI
    public bool CanGoBack => CurrentPage > 0 && CurrentPage < 2;
    public bool CanProceed => CurrentPage switch
    {
        0 => true,
        1 => HasSelections,
        _ => false
    };
    
    public string ActionButtonText => CurrentPage switch
    {
        0 => "Get Started →",
        1 => $"Install {SelectedCount} Apps",
        _ => "Next"
    };

    public MainWindowViewModel()
    {
        _ = LoadAppsAsync();
    }

    partial void OnCurrentPageChanged(int value)
    {
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanProceed));
        OnPropertyChanged(nameof(ActionButtonText));
    }

    private async Task LoadAppsAsync()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AppBundle.apps.json";
            
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                var resources = assembly.GetManifestResourceNames();
                System.Diagnostics.Debug.WriteLine($"Available resources: {string.Join(", ", resources)}");
                throw new FileNotFoundException($"Embedded resource '{resourceName}' not found");
            }

            var appCollection = await JsonSerializer.DeserializeAsync<AppCollection>(stream);
            if (appCollection?.Apps == null)
            {
                throw new InvalidOperationException("Failed to deserialize apps.json or apps list is null");
            }

            foreach (var app in appCollection.Apps)
            {
                var appViewModel = new AppItemViewModel(app);
                appViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(AppItemViewModel.IsSelected))
                    {
                        OnPropertyChanged(nameof(SelectedCount));
                        OnPropertyChanged(nameof(HasSelections));
                        OnPropertyChanged(nameof(CanProceed));
                        OnPropertyChanged(nameof(ActionButtonText));
                    }
                };
                Apps.Add(appViewModel);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading apps: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    [RelayCommand]
    private void Next()
    {
        if (CurrentPage < 3)
            CurrentPage++;
    }
    
    [RelayCommand]
    private void Action()
    {
        // Unified action button command
        if (CurrentPage == 0)
        {
            Next();
        }
        else if (CurrentPage == 1 && HasSelections)
        {
            _ = Install();
        }
    }

    [RelayCommand]
    private void Back()
    {
        if (CurrentPage > 0)
            CurrentPage--;
    }

    [RelayCommand]
    private void Cancel()
    {
        Environment.Exit(0);
    }

    [RelayCommand]
    private async Task Install()
    {
        if (!HasSelections) return;

        CurrentPage = 2; // Progress page
        IsIndeterminate = false;

        var selectedApps = Apps.Where(a => a.IsSelected).ToList();
        var totalApps = selectedApps.Count;
        var currentAppIndex = 0;

        InstallLogs.Clear();
        InstallLogs.Add($"Starting installation of {totalApps} applications...");

        foreach (var appViewModel in selectedApps)
        {
            currentAppIndex++;
            OverallProgress = (int)((currentAppIndex - 1) / (double)totalApps * 100);
            StatusText = $"Installing {appViewModel.Name} ({currentAppIndex} of {totalApps})...";
            
            InstallLogs.Add($"[{DateTime.Now:HH:mm:ss}] Installing {appViewModel.Name}...");

            var progress = new Progress<string>(status =>
            {
                StatusText = status;
                InstallLogs.Add($"[{DateTime.Now:HH:mm:ss}] {status}");
            });

            await installationService.InstallAppAsync(appViewModel, progress);

            if (appViewModel.Status == InstallStatus.Done)
            {
                InstallLogs.Add($"[{DateTime.Now:HH:mm:ss}] ✓ {appViewModel.Name} installed successfully");
            }
            else if (appViewModel.Status == InstallStatus.Failed)
            {
                InstallLogs.Add($"[{DateTime.Now:HH:mm:ss}] ✗ {appViewModel.Name} failed: {appViewModel.StatusText}");
            }
        }

        OverallProgress = 100;
        StatusText = "Installation complete!";
        
        var successCount = selectedApps.Count(a => a.Status == InstallStatus.Done);
        var failCount = selectedApps.Count(a => a.Status == InstallStatus.Failed);
        
        SuccessMessage = successCount == totalApps 
            ? $"Successfully installed all {successCount} applications!" 
            : $"Installed {successCount} of {totalApps} applications. {failCount} failed.";

        await Task.Delay(1000);
        CurrentPage = 3; // Finish page
    }

    [RelayCommand]
    private void Finish()
    {
        Environment.Exit(0);
    }
}