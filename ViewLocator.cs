using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AppBundle.Avalonia.ViewModels;
using System;

namespace AppBundle.Avalonia;

/// <summary>
/// Locates and instantiates views based on view models
/// </summary>
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data == null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            var instance = Activator.CreateInstance(type);
            if (instance is Control control)
            {
                return control;
            }
        }

        return new TextBlock { Text = $"View not found: {name}" };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}