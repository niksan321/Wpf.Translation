namespace Wpf.Tr;

public static class EnumExtensions
{
    public static string GetTranslateKey(this Enum value)
    {
        var attribute = value.GetType()
            .GetField(value.ToString())!
            .GetCustomAttributes(typeof(TrAttribute), false)
            .Cast<TrAttribute>()
            .SingleOrDefault();

        return attribute?.Key;
    }
}