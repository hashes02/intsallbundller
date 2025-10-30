using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AppBundle.Avalonia.ViewModels;
using AppBundle.Avalonia.Views;

namespace AppBundle.Avalonia;

/// <summary>
/// Main application class
/// </summary>
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Remove Avalonia data validation to avoid duplicates with CommunityToolkit
            BindingPlugins.DataValidators.RemoveAt(0);
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}