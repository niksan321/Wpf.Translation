namespace Wpf.Tr;

/// <summary>
/// Translate attribute
/// </summary>
public class TrAttribute : Attribute
{
    public string Key { get; set; }

    public TrAttribute(string key)
    {
        Key = key;
    }
}