namespace Wpf.Translation;

public static class EnumExtensions
{
    public static string GetTranslateKey(this Enum value)
    {
        var attribute = value.GetType()
            .GetField(value.ToString())!
            .GetCustomAttributes(typeof(LocalizeAttribute), false)
            .Cast<LocalizeAttribute>()
            .SingleOrDefault();

        return attribute?.Key;
    }
}