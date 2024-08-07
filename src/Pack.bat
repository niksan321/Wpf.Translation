if not exist "Packages" (mkdir "Packages") else (del /F /Q "Packages\*")
dotnet restore Wpf.Translation.sln
dotnet msbuild /t:build /p:Configuration=Release /p:GeneratePackageOnBuild=false /p:ExcludeGeneratedDebugSymbol=false Wpf.Translation.sln
dotnet pack -c Release -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -o Packages Wpf.Translation.sln
pause