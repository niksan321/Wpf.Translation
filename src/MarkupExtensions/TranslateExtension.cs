using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.Translation;

public class TranslateExtension : MarkupExtension
{
    [ConstructorArgument("key")]
    public string Key { get; set; }

    public Binding Binding { get; set; }

    public TranslateExtension() { }

    public TranslateExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetService(typeof(IProvideValueTarget));
        if (service is IProvideValueTarget { TargetObject: DependencyObject, TargetProperty: DependencyProperty } target)
        {
            var targetObject = (DependencyObject)target.TargetObject;
            var targetProperty = (DependencyProperty)target.TargetProperty;

            return Key == null
                ? serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Binding)
                : serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Key);
        }

        return this;
    }
}