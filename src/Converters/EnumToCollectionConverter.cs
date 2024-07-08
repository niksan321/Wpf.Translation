﻿using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.Translation;

[ValueConversion(typeof(Enum), typeof(IEnumerable<EnumDisplayName>))]
public class EnumToItemsSource : MarkupExtension
{
    public class EnumDisplayName
    {
        public Enum Value { get; set; }
        public string DisplayName { get; set; }
    }

    private readonly Type _type;

    public EnumToItemsSource(Type type)
    {
        _type = type;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(_type)
            .Cast<Enum>()
            .Select(e => new EnumDisplayName { Value = e, DisplayName = e.GetTranslateKey() });
    }
}