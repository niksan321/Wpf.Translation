using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Wpf.Tr;

public static class Ex
{
    #region TranslateProperty

    public static readonly DependencyProperty TranslateProperty = DependencyProperty.RegisterAttached(
        "Translate", typeof(bool), typeof(Ex), new PropertyMetadata(false, PropertyChangedCallback));

    public static void SetTranslate(DependencyObject element, bool value)
    {
        element.SetValue(TranslateProperty, value);
    }

    public static bool GetTranslate(DependencyObject element)
    {
        return (bool)element.GetValue(TranslateProperty);
    }

    #endregion

    private static readonly List<DataGrid> _dataGrids = new();

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DataGrid dg || e.NewValue == null) return;

        dg.Language = XmlLanguage.GetLanguage(TranslateManager.Instance.CurrentLanguage?.IetfLanguageTag ?? "en-US");

        if ((bool)e.NewValue)
        {
            dg.Loaded += DataGrid_Loaded;
            dg.Unloaded += DataGrid_Unloaded;
        }
        else
        {
            if (_dataGrids.Contains(dg))
            {
                dg.Loaded -= DataGrid_Loaded;
                dg.Unloaded -= DataGrid_Unloaded;
                _dataGrids.Remove(dg);
            }

            if (_dataGrids.Count == 0)
            {
                TranslateManager.Instance.LanguageChanged -= LanguageChanged;
            }
        }
    }

    private static void DataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        var dg = (DataGrid)sender;
        _dataGrids.Add(dg);

        if (_dataGrids.Count == 1)
        {
            TranslateManager.Instance.LanguageChanged += LanguageChanged;
        }

        if (dg.Language.IetfLanguageTag != TranslateManager.Instance.CurrentLanguage?.IetfLanguageTag)
        {
            ResetItemsSource(dg);
        }
    }

    private static void DataGrid_Unloaded(object sender, RoutedEventArgs e)
    {
        _dataGrids.Remove((DataGrid)sender);

        if (_dataGrids.Count == 0)
        {
            TranslateManager.Instance.LanguageChanged -= LanguageChanged;
        }
    }

    private static void LanguageChanged(object sender, EventArgs e)
    {
        foreach (var dg in _dataGrids)
        {
            dg.Language = XmlLanguage.GetLanguage(TranslateManager.Instance.CurrentLanguage.IetfLanguageTag);
            ResetItemsSource(dg);
        }
    }

    private static void ResetItemsSource(DataGrid dg)
    {
        var itemsSource = dg.ItemsSource;
        dg.ItemsSource = null;
        dg.ItemsSource = itemsSource;
    }
}