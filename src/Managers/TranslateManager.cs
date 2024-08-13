using System.Globalization;
using System.Resources;

namespace Wpf.Tr;

/// <summary>
/// Translation manager
/// </summary>
public class TranslateManager
{
    private readonly List<ResourceManager> _languageResources;

    public static CultureInfo[] Languages { get; private set; }

    public event EventHandler LanguageChanged;

    private CultureInfo _currentLanguage;
    public CultureInfo CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (!Equals(_currentLanguage, value))
            {
                _currentLanguage = value;
                Thread.CurrentThread.CurrentUICulture = value;
                RiseLanguageChanged();
            }
        }
    }

    public static TranslateManager Instance { get; private set; }

    public TranslateManager()
    {
        _languageResources = new List<ResourceManager>();
        Instance = this;
    }

    public CultureInfo GetSystemOrDefaultLanguage()
    {
        var systemLang = CultureInfo.CurrentUICulture;
        return Languages.Contains(systemLang) ? systemLang : Languages[0];
    }

    public void SetLanguageFromSystem()
    {
        CurrentLanguage = GetSystemOrDefaultLanguage();
    }

    private void RiseLanguageChanged()
    {
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    public string Translate(string key, params object[] p)
    {
        var translatedValue = TranslateValue(key);
        if (translatedValue != null)
        {
            try
            {
                return string.Format(translatedValue, p);
            }
            catch (FormatException)
            {
                throw new FormatException(@$"
                    Format exception occur. 
                    Check localized string and number of given parameters.
                    key=`{key}`
                    str=`{translatedValue}`,
                    params=`{p.ParamsToString()}`");
            }
        }

        return $"!{key}!";
    }

    public void RegisterResourceManager(ResourceManager manager)
    {
        _languageResources.Add(manager);
    }

    public void InitSupportedLanguages(CultureInfo[] languages)
    {
        Languages = languages;
    }

    private string TranslateValue(string key)
    {
        if (key == null) return "Null";

        foreach (var dict in _languageResources)
        {
            var r = dict.GetString(key, _currentLanguage);
            if (r != null) return r;
        }

        return null;
    }
}