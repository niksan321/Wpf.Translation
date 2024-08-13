using BindingGrid.Enums;
using BindingGrid.Models;

namespace BindingGrid.Helpers;

public static class DataSeeder
{
    public static Target[] SeedTargets()
    {
        var targets = new List<Target>
        {
            new()
            {
                Id = 1,
                Type = RequestType.HttpGet,
                Data = "Обычные не локализованные данные"
            },
            new()
            {
                Id = 2,
                Type = RequestType.HttpPost,
                Data = "Some not localized data"
            },
            new()
            {
                Id = 3,
                Type = RequestType.IcmpPing,
                Data = "12345678"
            }
        };

        return targets.ToArray();
    }
}