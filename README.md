# <img src="icon.png" width="71" height="71"/> ACR Notifications Plugin for Xamarin and Windows
_Plugin for Easy Cross Platform notifications_

### [SUPPORT THIS PROJECT](https://github.com/aritchie/home)

[Change Log - October 27, 2018](changelog.md)

[![Build status](https://dev.azure.com/allanritchie/Plugins/_apis/build/status/Notifications)](https://dev.azure.com/allanritchie/Plugins/_build/latest?definitionId=0)
[![NuGet](https://img.shields.io/nuget/v/Plugin.Notifications.svg?maxAge=2592000)](https://www.nuget.org/packages/Plugin.Notifications/)

## FEATURES

* Local Notifications
* Triggers
    * Time Interval Based
    * Geofence Location Based
    * Calendar Based (Specific Date, Day of Week, Week of Year)
* [Sounds](sounds.md)
* Read All Scheduled Notifications
* Badges
* Vibrate Device (Android & iOS only)
* Set payload on notification for additional data
* Cancel specific or all notifications
* Android Specific
    * Survives reboots
    * Channels, Colors
    * Priorities
    * Icon Control


Platform|Version
--------|-------
Android|5.0+
iOS|9+
Windows UWP|16299+
Any Other Platform|Must Support .NET Standard 2.0

### SETUP

Install the nuget package in your platform project as well as your netstandard library.

* [Android](platform_android.md)
* [iOS](platform_ios.md)
* [Windows](platform_uwp.md)


### EXAMPLES

### Send a notification

```csharp
await CrossNotifications.Current.Send("My Title", "My message for the notification");
```

### Send a scheduled notification:

```csharp
await CrossNotifications.Current.Send("Happy Birthday", "I sent this a long time ago", when = TimeSpan.FromDays(50));
```

### Get a list of pending notifications

```csharp
var list = await CrossNotifications.Current.GetPendingNotifications();
```

### Cancel a specific notification
```csharp
var id = await CrossNotifications.Current.Send("Hi", "This is my scheduled notification", when = TimeSpan.FromDays(1));
await CrossNotifications.Current.Cancel(id);
```

### Cancel all scheduled notifications and clear badge:

```csharp
CrossNotifications.Current.CancelAll();
```

### To set a badge:

Setting badges works on all platforms, though only select flavours of Android.  A 3rd party library is used to accomplish this.

```csharp
await CrossNotifications.Current.SetBadge(4);
await CrossNotifications.Current.GetBadge();
// 0 clears badge
```

### Listen for Event Activations

You can have your app listen for taps on notifications.  You should install this hook in the startup of your application.
Such as App.cs in Xamarin.Forms, AppDelegate on iOS or your launch activity/application in Android

PLEASE NOTE: Activated does not work on scheduled notifications on UWP

```csharp
CrossNotifications.Current.Activated += (sender, notification) => {
    // work with notification
};
```

## Contributors

* **[Jelle Damen](https://twitter.com/JelleDamen)** for the wonderful icons