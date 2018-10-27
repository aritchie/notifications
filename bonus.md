## BONUS FUNCTIONALITY

### Vibrate
```csharp
CrossNotifications.Current.Vibrate();
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