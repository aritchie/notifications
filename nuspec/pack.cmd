@echo off
del *.nupkg
nuget pack Plugin.Notifications.nuspec
pause