using Avalonia.Controls;
using Avalonia.Input;
using AppBundle.Avalonia.ViewModels;

namespace AppBundle.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnAppCardPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is AppItemViewModel appViewModel)
        {
            appViewModel.IsSelected = !appViewModel.IsSelected;
        }
    }
}