nuget restore -SolutionDirectory ../  ../Rock.Encryption\RockLib.Encryption.csproj

msbuild /p:Configuration=Release /t:Clean ..\Rock.Encryption\RockLib.Encryption.csproj

msbuild /p:Configuration=Release /t:Rebuild ..\Rock.Encryption\RockLib.Encryption.csproj

msbuild /t:pack /p:PackageOutputPath=..\..\..\builtPackages  /p:Configuration=Release ..\Rock.Encryption\RockLib.Encryption.csproj