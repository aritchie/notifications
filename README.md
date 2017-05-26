# ACR Notifications Plugin for Xamarin and Windows

Plugin for Easy Cross Platform notifications

[![NuGet](https://img.shields.io/nuget/v/Acr.Notifications.svg?maxAge=2592000)](https://www.nuget.org/packages/Acr.Notifications/)
[Change Log - May 26, 2017](changelog.md)


## Features

* Local Notifications
* Scheduled Notifications
* Sounds
* Read all Scheduled Notifications
* Badges
  * iOS
  * Android


## Supported OS
* iOS 6+
* Android 4+
* Universal Windows Platform (Win10/UWP)
* NET Standard 1.0

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

### To set a badge (excluding android):
```csharp
CrossNotifications.Current.Badge = 4; // TODO: 0 clears badge
```


### Sounds

_In the notification.Sound property - set only the filename without the extension_

#### Android
* Put the sound in your /Resources/raw/ folder - make sure the file properties is set to _AndroidResource_

#### iOS
* Put the file in your iOS app bundle
* The file format must be a .caf file (google this to see how to make one) - ie. afconvert -f caff -d aacl@22050 -c 1 sound.aiff soundFileName.caf 

#### UWP
* Supports aac, flac, m4a, mp3, wav, & wma file formats
* For desktop v1511, custom audio will not work.  The plugin will ignore the sound config if it sees this.
* Read the following article for more info: https://blogs.msdn.microsoft.com/tiles_and_toasts/2016/06/18/quickstart-sending-a-toast-notification-with-custom-audio/