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

###Cancel all scheduled notifications and clear badge:

[warning] This will not cancel future scheduled notifications on Android.  Keep the notification IDs and cancel one-by-one
```csharp
CrossNotifications.Current.CancelAll();
```

###To set a badge (excluding android):
```csharp
CrossNotifications.Current.Badge = 4; // TODO: 0 clears badge
```