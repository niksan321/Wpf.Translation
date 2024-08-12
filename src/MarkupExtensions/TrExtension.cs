using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.Tr;

/// <summary>
/// Translate markup extension
/// </summary>
public class TrExtension : MarkupExtension
{
    [ConstructorArgument("key")]
    public string Key { get; set; }

    public Binding Binding { get; set; }

    public TrExtension() { }

    public TrExtension(string key)
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

            // if we don't have an incoming service provider then just return our translation binding instance
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget) return this;

            /*
             * if we don't have our DependencyObject then more than likely we have a System.Windows.SharedDp 
             * and the XAML processor will call us again with the appropriate value
             */
            if (provideValueTarget.TargetObject is not DependencyObject _) return this;

            // we don't need to translate anything if we are in design mode
            // todo add FallbackValue
            //if (DesignerProperties.GetIsInDesignMode(InternalTarget)) return InternalFallbackValue;

            return Key == null
                ? serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Binding)
                : serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Key);
        }

        return this;
    }
}