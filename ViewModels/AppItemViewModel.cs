using CommunityToolkit.Mvvm.ComponentModel;

namespace AppBundle.Avalonia.ViewModels;

/// <summary>
/// View model for individual application items
/// </summary>
public partial class AppItemViewModel : ViewModelBase
{
    private readonly AppInfo app;

    [ObservableProperty]
    private bool isSelected;

    [ObservableProperty]
    private InstallStatus status = InstallStatus.NotStarted;

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private string statusClass = "";

    public string Id => app.Id;
    public string Name => app.Name;
    public string Description => GetDescription(app.Source);

    public AppItemViewModel(AppInfo app)
    {
        this.app = app;
        IsSelected = app.IsSelected;
    }
    
    private static string GetDescription(string source)
    {
        return source switch
        {
            "winget" => "From Microsoft Store",
            "github" => "From GitHub Releases",
            "direct" => "Official Download",
            _ => "Latest Version"
        };
    }

    partial void OnStatusChanged(InstallStatus value)
    {
        StatusClass = value switch
        {
            InstallStatus.Downloading => "status-downloading",
            InstallStatus.Installing => "status-installing",
            InstallStatus.Done => "status-done",
            InstallStatus.Failed => "status-failed",
            InstallStatus.Skipped => "status-skipped",
            _ => ""
        };
    }
}