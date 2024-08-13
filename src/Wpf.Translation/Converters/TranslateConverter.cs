using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Tr;

/// <summary>
/// Translate converter
/// </summary>
[ValueConversion(typeof(string), typeof(string))]
public class TranslateConverter : FrameworkElement, IValueConverter, IWeakEventListener, IDisposable
{
    private readonly DependencyObject _targetObject;
    private readonly DependencyProperty _targetProperty;

    public TranslateConverter(DependencyObject targetObject, DependencyProperty targetProperty)
    {
        _targetObject = targetObject;
        _targetProperty = targetProperty;

        LanguageChangedEventManager.AddListener(TranslateManager.Instance, this);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) { return DependencyProperty.UnsetValue; }

        string result;
        string key = null;

        if (value is Enum enumValue)
        {
            key = enumValue.GetTranslateKey();
        }

        if (parameter is Array arr)
        {
            result = TranslateManager.Instance.Translate(key ?? value.ToString(), arr.Cast<object>().ToArray());
        }
        else
        {
            result = TranslateManager.Instance.Translate(key ?? value.ToString(), parameter);
        }

        return string.IsNullOrWhiteSpace(result) ? DependencyProperty.UnsetValue : result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
        if (managerType != typeof(LanguageChangedEventManager)) return false;

        Invalidate();
        return true;
    }

    private void Invalidate()
    {
        var expression = BindingOperations.GetBindingExpressionBase(_targetObject, _targetProperty);
        expression?.UpdateTarget();
    }

    ~TranslateConverter()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            LanguageChangedEventManager.RemoveListener(TranslateManager.Instance, this);
        }
    }
}