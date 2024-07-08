using System.Globalization;
using System.Resources;
using System.Transactions;

namespace Wpf.Translation;

public class TranslationManager
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
            if (!Equals(Thread.CurrentThread.CurrentUICulture, value))
            {
                _currentLanguage = value;
                Thread.CurrentThread.CurrentUICulture = value;
                RiseLanguageChanged();
            }
        }
    }

    public static TranslationManager Instance { get; private set; }

    public TranslationManager()
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
        var result = string.IsNullOrWhiteSpace(translatedValue) ? $"!{key}!" : string.Format(translatedValue, p);
        return result;
    }

    public string TranslateValue(string key)
    {
        if (key == null) return "Null";

        foreach (var dict in _languageResources)
        {
            var r = dict.GetString(key, _currentLanguage);
            if (r != null) return r;
        }

        return null;
    }

    public void RegisterResourceManager(ResourceManager manager)
    {
        _languageResources.Add(manager);
    }

    public void InitSupportedLanguages(CultureInfo[] languages)
    {
        Languages = languages;
    }
}