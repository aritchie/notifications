# ACR Notifications Plugin for Xamarin and Windows

---

## Features

* Local Notifications
* Sounds
* Scheduled Notifications
* Badges

## Supported OS
* iOS 6+
* Android 4+
* Universal Windows Platform (Win10/UWP)
* Portable Class Libraries (Profile 259)


### Installation

Install the nuget package in your platform project as well as your shared library.


### Send a notification

```csharp
CrossNotifications.Current.Send("My Title", "My message for the notification");
```

### Send a scheduled notification:

```csharp
CrossNotifications.Current.Send("Happy Birthday", "I sent this a long time ago", when = TimeSpan.FromDays(50));
```

### Cancel a specific notification
```csharp
var id = CrossNotifications.Current.Send("Hi", "This is my scheduled notification", when = TimeSpan.FromDays(1));
CrossNotifications.Current.Cancel(id);
```

### Cancel all scheduled notifications and clear badge:

[warning] This will not cancel future scheduled notifications on Android.  Keep the notification IDs and cancel one-by-one
```csharp
CrossNotifications.Current.CancelAll();
```

### To set a badge:
Badges work on iOS and Windows, but only certain flavours of Android.  Please take a Xamarin.ShortcutBadger for more info.
```csharp
CrossNotifications.Current.Badge = 4; // TODO: 0 clears badge
```


### Sounds

This library only forwards a path through to the native API.  These paths will
be vastly different between platforms

You can set a default sound:
```csharp
Notification.DefaultSound = "path";
```

#### Android
The path will be something like android.resource://!YOUR PACKAGE NAME!/raw/<your file without the extension>";  Go to https://stackoverflow.com/questions/13760168/how-to-set-notification-with-custom-sound-in-android for more info
Make sure the file is set as an AndroidResource in its file properties.

#### iOS
You must simply add a caf sound to the app bundle and call it with 
notification.Sound = "soundname.caf";
