## iOS

### Setup
1. Install From [![NuGet](https://img.shields.io/nuget/v/Plugin.Notifications.svg?maxAge=2592000)](https://www.nuget.org/packages/Plugin.Notifications/)

2. In your AppDelegate.cs, add the following:
```csharp

In FinishedLaunching method, add
Plugin.Notifications.CrossJobs.Init();
```

3. Add the following to your info.plist
```xml
<key>UIBackgroundModes</key>
<array>
	<string></string>
</array>
```
