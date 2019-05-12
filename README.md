# UPDATE - This library has now moved to the Shiny Framework at https://github.com/shinyorg/shiny



## ACR Notifications Plugin for Xamarin and Windows

Plugin for Easy Cross Platform notifications

[![NuGet](https://img.shields.io/nuget/v/Acr.Notifications.svg?maxAge=2592000)](https://www.nuget.org/packages/Acr.Notifications/)
[Change Log - July 28, 2017](changelog.md)


## Features

* Local Notifications
* Scheduled Notifications
* Sounds
* Read all Scheduled Notifications
* Badges
* Set metadata on each notification for identification
* Cancel individual or all notifications


## Supported OS
* iOS 6+
* macOS
* Android 4+
* Universal Windows Platform (Win10/UWP)
* NET Standard 1.0

### Installation

Install the nuget package in your platform project as well as your netstandard library.


### Send a notification

```csharp
await CrossNotifications.Current.Send("My Title", "My message for the notification");
```

### Send a scheduled notification:

```csharp
await CrossNotifications.Current.Send("Happy Birthday", "I sent this a long time ago", when = TimeSpan.FromDays(50));
```

### Get a list of scheduled notifications

```csharp
var list = await CrossNotifications.Current.GetScheduledNotifications();
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


### Sounds

_In the notification.Sound property - set only the filename without the extension_

#### Android
* Put the sound in your /Resources/raw/ folder - make sure the file properties is set to _AndroidResource_
* You can pass the actual full sound path OR just the name... the plugin will figure it out!

#### iOS
* Put the file in your iOS app bundle
* The file format must be a .caf file (google this to see how to make one) - ie. afconvert -f caff -d aacl@22050 -c 1 sound.aiff soundFileName.caf 

#### UWP
* Supports aac, flac, m4a, mp3, wav, & wma file formats
* For desktop v1511, custom audio will not work.  The plugin will ignore the sound config if it sees this.
* Read the following article for more info: https://blogs.msdn.microsoft.com/tiles_and_toasts/2016/06/18/quickstart-sending-a-toast-notification-with-custom-audio/


### FAQ
* Why are most methods async now?
* _iOS requires all UI based commands run on the UI thread.  Notifications are part of UIKit and thus have this requirement.  With all of my plugins, I try to manage the thread marshalling for you_

* Why can't I set a string as an identifier
* _Android needs an integer for how it sets identifiers_
