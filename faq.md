## FAQ

Q. Why don't my scheduled notifications fire at the exact minute I set them

> On android & UWP, we're limited to some of the rules there.  They usually fire within 5-10 minutes of your scheduled time.

Q. Will scheduled notifications continue to fire after device reboots

> Yes (make sure to follow the setup guide for Android)

Q. Do I need to request permissions for all platforms?

> For geofencing triggers, yes.  For simple notifications, only on iOS, but the library will assert this before sending a notification the first time anyhow, so you don't need to do it yourself if you don't want to.  However, the INotificationManager.RequestPermission will return true on the other environments

Q. Why are most methods async now?

> _iOS requires all UI based commands run on the UI thread.  Notifications are part of UIKit and thus have this requirement.  With all of my plugins, I try to manage the thread marshalling for you_

Q. Why can't I set a string as an identifier

> _Android needs an integer for how it sets identifiers_
