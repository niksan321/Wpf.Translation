namespace Wpf.Translation;

public class LocalizeAttribute : Attribute
{
    public string Key { get; set; }

    public LocalizeAttribute(string key)
    {
        Key = key;
    }
}