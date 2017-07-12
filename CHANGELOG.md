# CHANGE LOG

## 4.0
* [breaking] Acr.Notifications => Plugin.Notifications
* [breaking] Most methods are now async to deal with iOS' need to have notifications run on the main thread
* [feature] package is now NET standard 1.0 compliant
* [feature] ability to set your own identifier 
* [feature] get list of scheduled notifications
* [feature] sound manage is 100x easier now.  explained in readme
* [feature] macOS support

## 3.1.2
* [fix][ios] alert title not set properly

## 3.1.1
* [fix][uwp] issue with notification id not incrementing

## 3.1
* nuget package updates

## 3.0
* [fix][uwp] scheduled notifications with id are fixed
* [breaking] Notifications.Instance is now CrossNotifications.Current
* [breaking] WP8+ and Win8 are gone for good

## 2.0.1
* allow json.net 10 updates

## 2.0
* [feature][uwp] UWP has arrived
* [feature][droid] now has appicon badges thanks to neurospeech ShortcutBadger bindings!
* [feature][droid] request for vibrate on notification (not supported on other platforms)

## 1.5.2
* [droid] loosening up support lib requirement

## 1.5.1
* [droid][fix] remove dependency on resources and allow for updates to support libs
* [update] update nuget packages

## 1.5
* [droid][fix] scheduled notifications
* [droid][fix] cancel all scheduled notifications
* [droid][fix] launches main activity
* [droid][feature] NotificationsImpl.AppIconResourceId = your resourceId for the small icon

## 1.4
* [pcl] update json.net
* [breaking] remove context menus - they just didn't live up to the hype

## 1.3
* [ios][fix] fix crash with messageID
* [ios][feature] notification actions (coming soon to android)
* [android][fix] more scheduling issues fixed on android

## 1.2
* [android] fix scheduled notifications
* [pcl] default sound and title for notifications
* [pcl] improved interface

## 1.1
* [android] Sounds have been added
* [feature] Ability to cancel specific notifications (Send returns a notification ID)

## 1.0
* Initial Release