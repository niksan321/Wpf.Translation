using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Tr;

public static class ServiceProviderExtensions
{
    public static object ProvideLocalizedValue(this IServiceProvider serviceProvider, DependencyObject targetObject,
        DependencyProperty targetProperty, string key)
    {
        var bindingToString = new Binding
        {
            Source = key,
            Mode = BindingMode.OneWay,
            Converter = new TrConverter(targetObject, targetProperty)
        };

        return bindingToString.ProvideValue(serviceProvider);
    }

    public static object ProvideLocalizedValue(this IServiceProvider serviceProvider, DependencyObject targetObject,
        DependencyProperty targetProperty, Binding Key)
    {
        var fieldInfo = typeof(BindingBase).GetField("_isSealed", BindingFlags.NonPublic | BindingFlags.Instance);
        var isSealed = (bool)fieldInfo!.GetValue(Key)!;

        //hack to update sealed binding properties
        if (isSealed) fieldInfo.SetValue(Key, false);

        Key.Mode = BindingMode.OneWay;
        Key.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        Key.Converter = new TrConverter(targetObject, targetProperty);

        if (isSealed) fieldInfo.SetValue(Key, true);

        return Key.ProvideValue(serviceProvider);
    }
}