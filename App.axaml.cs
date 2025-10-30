using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AppBundle.Avalonia.ViewModels;
using AppBundle.Avalonia.Views;
using System;

namespace AppBundle.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        try
        {
            AvaloniaXamlLoader.Load(this);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing XAML: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
            throw;
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error completing framework initialization: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
            throw;
        }
    }
}