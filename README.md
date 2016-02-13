#ACR Notifications Plugin for Xamarin and Windows

---

##Features

* Local Notifications
* Sounds
* Scheduled Notifications
* Badges


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