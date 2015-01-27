mkdir input\lib\net45
del /Q input\lib\net45\*.*

msbuild .\..\src\MvcRouteTester\MvcRouteTester.csproj /p:Configuration=Release;OutputPath=.\..\..\build\input\lib\net45

mkdir output
..\.nuget\nuget.exe pack /o output .\MvcLocalizedRouteTester.nuspec