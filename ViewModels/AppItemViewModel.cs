using CommunityToolkit.Mvvm.ComponentModel;

namespace AppBundle.Avalonia.ViewModels;

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

    public AppItemViewModel(AppInfo app)
    {
        this.app = app;
        IsSelected = app.IsSelected;
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