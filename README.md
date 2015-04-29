ACR Notifications Plugin for Xamarin and Windows
===

Installation:
---
Install the nuget package in your platform project as well as your shared library.

Send a notification:

    Notifications.Instance.Send("My Title", "My message for the notification");

Send a scheduled notification:

    Notifications.Instance.Send("Happy Birthday", "I sent this a long time ago", when = TimeSpan.FromDays(50));

Cancel all scheduled notifications and clear badge:

    Notifications.Instance.CancelAll();

To set a badge (excluding android):

    Notifications.Instance.Badge = 4; // TODO: 0 clears badge