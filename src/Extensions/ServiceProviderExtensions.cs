using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Tr;

public static class ServiceProviderExtensions
{
    public static object ProvideLocalizedValue(this IServiceProvider serviceProvider, DependencyObject targetObject,
        DependencyProperty targetProperty, string key, params object[] p)
    {
        var bindingToString = new Binding
        {
            Source = key,
            Mode = BindingMode.OneWay,
            ConverterParameter = p,
            Converter = new TranslateConverter(targetObject, targetProperty)
        };

        return bindingToString.ProvideValue(serviceProvider);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used",
        Justification = "to update sealed binding properties")]
    public static object ProvideLocalizedValue(this IServiceProvider serviceProvider, DependencyObject targetObject,
        DependencyProperty targetProperty, Binding key, params object[] p)
    {
        var fieldInfo = typeof(BindingBase).GetField("_isSealed", BindingFlags.NonPublic | BindingFlags.Instance);
        var isSealed = (bool)fieldInfo!.GetValue(key)!;

        //hack to update sealed binding properties
        if (isSealed) fieldInfo.SetValue(key, false);

        key.Mode = BindingMode.OneWay;
        key.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        key.ConverterParameter = p;
        key.Converter = new TranslateConverter(targetObject, targetProperty);

        if (isSealed) fieldInfo.SetValue(key, true);

        return key.ProvideValue(serviceProvider);
    }
}