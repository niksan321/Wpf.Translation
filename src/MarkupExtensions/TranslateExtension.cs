using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.Tr;

/// <summary>
/// Translate markup extension
/// </summary>
public class TranslateExtension : MarkupExtension
{
    /// <summary>
    /// Localization key
    /// </summary>
    [ConstructorArgument("key")]
    public string Key { get; set; }

    /// <summary>
    /// Localization binding to key property
    /// </summary>
    public Binding Binding { get; set; }

    /// <summary>
    /// Localization parameter 1
    /// </summary>
    public object P1 { get; set; }

    /// <summary>
    /// Localization parameter 2
    /// </summary>
    public object P2 { get; set; }

    /// <summary>
    /// Localization parameter 3
    /// </summary>
    public object P3 { get; set; }

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
                ? serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Binding, P1, P2, P3)
                : serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Key, P1, P2, P3);
        }

        return this;
    }
}