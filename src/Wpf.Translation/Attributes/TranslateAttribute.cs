namespace Wpf.Tr;

/// <summary>
/// Translate attribute
/// </summary>
public class TranslateAttribute : Attribute
{
    public string Key { get; set; }

    public TranslateAttribute(string key)
    {
        Key = key;
    }
}