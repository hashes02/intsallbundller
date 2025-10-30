using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.Json;
using AppBundle.Avalonia.Services;

namespace AppBundle.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly InstallationService installationService = new();

    [ObservableProperty]
    private string title = "AppBundle";

    [ObservableProperty]
    private string subtitle = "Install essential applications with one click";

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private string installButtonText = "Select applications to install";

    [ObservableProperty]
    private bool isInstalling = false;

    [ObservableProperty]
    private bool canInstall = false;

    public ObservableCollection<AppItemViewModel> Apps { get; } = new();
    public ObservableCollection<AppItemViewModel> FilteredApps { get; } = new();

    public MainWindowViewModel()
    {
        // Add test data immediately for debugging
        AddTestData();
        _ = LoadAppsAsync();
    }

    private void AddTestData()
    {
        // Add some test apps to verify UI is working
        var testApps = new[]
        {
            new AppInfo { Id = "test1", Name = "Test Application 1", IsSelected = true },
            new AppInfo { Id = "test2", Name = "Test Application 2", IsSelected = false },
            new AppInfo { Id = "test3", Name = "Test Application 3", IsSelected = false }
        };

        foreach (var app in testApps)
        {
            var appViewModel = new AppItemViewModel(app);
            appViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AppItemViewModel.IsSelected))
                {
                    UpdateInstallButton();
                }
            };
            Apps.Add(appViewModel);
            FilteredApps.Add(appViewModel);
        }
        
        UpdateInstallButton();
    }

    private async Task LoadAppsAsync()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("AppBundle.Avalonia.apps.json");
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                var appCollection = JsonSerializer.Deserialize<AppCollection>(json);
                if (appCollection != null)
                {
                    foreach (var app in appCollection.Apps)
                    {
                        var appViewModel = new AppItemViewModel(app);
                        appViewModel.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(AppItemViewModel.IsSelected))
                            {
                                UpdateInstallButton();
                            }
                        };
                        Apps.Add(appViewModel);
                        FilteredApps.Add(appViewModel);
                    }
                }
            }
            UpdateInstallButton();
        }
        catch (Exception ex)
        {
            // Log error - for now just ignore
            System.Diagnostics.Debug.WriteLine($"Error loading apps: {ex.Message}");
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterApps();
    }

    private void FilterApps()
    {
        FilteredApps.Clear();
        var filtered = string.IsNullOrWhiteSpace(SearchText) 
            ? Apps 
            : Apps.Where(app => app.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var app in filtered)
        {
            FilteredApps.Add(app);
        }
    }

    [RelayCommand]
    private async Task InstallSelectedApps()
    {
        if (IsInstalling) return;

        var selectedApps = Apps.Where(a => a.IsSelected).ToList();
        if (!selectedApps.Any()) return;

        IsInstalling = true;
        InstallButtonText = "Installing...";

        try
        {
            // Install apps sequentially to avoid conflicts
            foreach (var appViewModel in selectedApps)
            {
                var progress = new Progress<string>(status =>
                {
                    // Progress updates are handled in the InstallationService
                });

                await installationService.InstallAppAsync(appViewModel, progress);
            }
        }
        finally
        {
            IsInstalling = false;
            UpdateInstallButton();
        }
    }

    private void UpdateInstallButton()
    {
        var selectedCount = Apps.Count(a => a.IsSelected);
        CanInstall = selectedCount > 0 && !IsInstalling;

        if (IsInstalling)
        {
            InstallButtonText = "Installing...";
        }
        else if (selectedCount == 0)
        {
            InstallButtonText = "Select applications to install";
        }
        else if (selectedCount == 1)
        {
            InstallButtonText = "Install 1 application";
        }
        else
        {
            InstallButtonText = $"Install {selectedCount} applications";
        }
    }
}