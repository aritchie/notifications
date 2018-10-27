## BASICS

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
