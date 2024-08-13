using Wpf.Tr;

namespace BindingGrid.Enums;

public enum RequestType
{
    [Translate("RequestTypePing")]
    IcmpPing,
    [Translate("RequestTypeGet")]
    HttpGet,
    [Translate("RequestTypePost")]
    HttpPost
}