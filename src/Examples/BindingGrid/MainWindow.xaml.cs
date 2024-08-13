using System.Globalization;
using System.Windows;
using BindingGrid.Helpers;
using BindingGrid.Models;
using Wpf.Tr;

namespace BindingGrid;

public partial class MainWindow : Window
{
    public List<Target> Targets { get; set; } = new List<Target>();
    private readonly TranslateManager _translateManager;

    public MainWindow()
    {
        _translateManager = InitTranslateManager();

        Targets.AddRange(DataSeeder.SeedTargets());

        DataContext = this;

        InitializeComponent();

        //Localization by code
        LocalizedByCodeLabel.Content = _translateManager.Translate("LocalizedByCodeContent", 111, "Костик", "Bar");
    }

    private TranslateManager InitTranslateManager()
    {
        var manager = new TranslateManager();

        var langs = new CultureInfo[]
        {
            new("ru-RU"),
            new("en-US"),
        };

        manager.InitSupportedLanguages(langs);
        manager.RegisterResourceManager(Langs.ResourceManager);

        return manager;
    }

    private bool _isFirst = true;
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var lang = _isFirst ? TranslateManager.Languages.First() : TranslateManager.Languages.Last();
        _translateManager.CurrentLanguage = lang;
        _isFirst = !_isFirst;
    }
}