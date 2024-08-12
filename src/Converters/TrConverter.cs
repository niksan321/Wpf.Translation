using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Tr;

/// <summary>
/// Translate converter
/// </summary>
[ValueConversion(typeof(string), typeof(string))]
public class TrConverter : FrameworkElement, IValueConverter, IWeakEventListener, IDisposable
{
    private readonly DependencyObject _targetObject;
    private readonly DependencyProperty _targetProperty;

    public TrConverter(DependencyObject targetObject, DependencyProperty targetProperty)
    {
        _targetObject = targetObject;
        _targetProperty = targetProperty;

        LanguageChangedEventManager.AddListener(TrManager.Instance, this);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) { return DependencyProperty.UnsetValue; }

        string result;

        if (value is Enum enumValue)
        {
            var key = enumValue.GetTranslateKey();
            result = TrManager.Instance.Translate(key ?? value.ToString(), parameter);
        }
        else
        {
            result = TrManager.Instance.Translate(value.ToString(), parameter);
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

    ~TrConverter()
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
            LanguageChangedEventManager.RemoveListener(TrManager.Instance, this);
        }
    }
}