using System.Text;

namespace Wpf.Tr;

public static class ArrayExtensions
{
    public static string ParamsToString(this object[] parameters)
    {
        var result = new StringBuilder();

        for (var i = 0; i < parameters.Length; i++)
        {
            result.Append($"Param{i}={parameters[i] ?? "Null"};");
        }

        return result.ToString();
    }
}