#ACR Notifications Plugin for Xamarin and Windows

---

###Installation:


Install the nuget package in your platform project as well as your shared library.


###Send a notification:

    Notifications.Instance.Send("My Title", "My message for the notification");


###Send a scheduled notification:

    Notifications.Instance.Send("Happy Birthday", "I sent this a long time ago", when = TimeSpan.FromDays(50));


###Cancel a specific notification

    var id = Notifications.Instance.Send("Hi", "This is my scheduled notification", when = TimeSpan.FromDays(1));
    Notifications.Instance.Cancel(id);


###Cancel all scheduled notifications and clear badge:

[warning] This will not cancel future scheduled notifications on Android.  Keep the notification IDs and cancel one-by-one

    Notifications.Instance.CancelAll();


###To set a badge (excluding android):

    Notifications.Instance.Badge = 4; // TODO: 0 clears badge


###Notification Actions (iOS only)

There is no way to handle actions directly with this implementation, you must override HandleAction off your AppDelegate

    Notifications.Instance.Send(new Notification("Test Title", "Test Message")
        .AddAction("Title", "Identifier")
    );