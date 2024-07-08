using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.Translation;

public class TranslateBindExtension : MarkupExtension
{
    [ConstructorArgument("bind")]
    public Binding Bind { get; set; }

    public TranslateBindExtension(Binding bind)
    {
        Bind = bind;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetService(typeof(IProvideValueTarget));
        if (service is IProvideValueTarget { TargetObject: DependencyObject, TargetProperty: DependencyProperty } target)
        {
            var targetObject = (DependencyObject)target.TargetObject;
            var targetProperty = (DependencyProperty)target.TargetProperty;

            return serviceProvider.ProvideLocalizedValue(targetObject, targetProperty, Bind);
        }

        return this;
    }
}