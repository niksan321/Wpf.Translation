using System.Windows;

namespace Wpf.Tr;

public class LanguageChangedEventManager : WeakEventManager
{
    public static void AddListener(TranslateManager source, IWeakEventListener listener)
    {
        CurrentManager.ProtectedAddListener(source, listener);
    }

    public static void RemoveListener(TranslateManager source, IWeakEventListener listener)
    {
        CurrentManager.ProtectedRemoveListener(source, listener);
    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        DeliverEvent(sender, e);
    }

    protected override void StartListening(object source)
    {
        var manager = (TranslateManager)source;
        manager.LanguageChanged += OnLanguageChanged;
    }

    protected override void StopListening(object source)
    {
        var manager = (TranslateManager)source;
        manager.LanguageChanged -= OnLanguageChanged;
    }

    private static LanguageChangedEventManager CurrentManager
    {
        get
        {
            var managerType = typeof(LanguageChangedEventManager);
            var manager = (LanguageChangedEventManager)GetCurrentManager(managerType);

            if (manager != null) return manager;

            manager = new LanguageChangedEventManager();
            SetCurrentManager(managerType, manager);
            return manager;
        }
    }
}