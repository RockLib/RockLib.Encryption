msbuild /p:Configuration=Release ..\Rock.Encryption\Rock.Encryption.csproj
nuget pack ..\Rock.Encryption\Rock.Encryption.csproj -Properties Configuration=Release