using BindingGrid.Enums;

namespace BindingGrid.Models;

public class Target
{
    public RequestType Type { get; set; }

    public string Data { get; set; }

    public int Id { get; set; }
}