@echo off
del *.nupkg
rem Acr.Notifications.nuspec
nuget pack Plugin.Notifications.nuspec
pause