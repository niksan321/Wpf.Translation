# Wpf.Translation
- WPF Localisation package that allow language switching on the fly
- Binding supported

# Usages
## Initialization
1. Add nuget package Wpf.Translation (by niksan321)
2. Add language resource file to project - Langs.resx
3. Add some localizations to Langs.resx
4. Create TranslateManager:
`var manager = new TranslateManager();`
5. Add supported langs:
`var langs = new CultureInfo[] { new("ru-RU"), new("en-US") };`
6. Init langs: 
`manager.InitSupportedLanguages(langs);`
7. Register resource files: 
`manager.RegisterResourceManager(Langs.ResourceManager);`

## Use in XAML
1. Add namespace
`xmlns:tr="clr-namespace:Wpf.Tr;assembly=Wpf.Translation"`
2. Use translation on dependency prop with static key - `MainWindow`
`Title="{tr:Translate MainWindow}"`
3. Use binding (Type contains languaje key)
`Text="{tr:Translate Binding={Binding Type}}"`
4. To use binding with data grid column:
- Add AttachedProperty to data grid
- `<DataGrid tr:Ex.Translate="True">`
- And add binding:
- `<DataGridTemplateColumn Header="{tr:Translate Type}">
    <DataGridTemplateColumn.CellTemplate>
        <DataTemplate>
            <StackPanel>
                <TextBlock Padding="8" Text="{tr:Translate Binding={Binding Type}}" />
            </StackPanel>
        </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>`

## Use in code
Localized string in resource file Langs.resx:
- `LocalizedByCodeContent` = `Localized by code content. Param 1 = {0}, Param 2 = {1}, Param 3 = {2}`
Use in code
- `LocalizedByCodeLabel.Content = _translateManager.Translate("LocalizedByCodeContent", 111, "Костик", "Bar");`

## Switch language
`var lang = TranslateManager.Languages.First();`
`_translateManager.CurrentLanguage = lang;`

# Todo
- Binding converter parameters
- Add Interfaces