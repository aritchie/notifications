## SOUNDS

_In the notification.Sound property - set only the filename without the extension_
You can also set Notification.DefaultSound (static) to a default config value

#### Android
* Put the sound in your /Resources/raw/ folder - make sure the file properties is set to _AndroidResource_
* You can pass the actual full sound path OR just the name... the plugin will figure it out!
* You can set your launch activity flags using AndroidConfig.LaunchActivityFlags = ActivityFlags.NewTask | ActivityFlags.ClearTask (or whatever you want)
* 
#### iOS
* Put the file in your iOS app bundle
* The file format must be a .caf file (google this to see how to make one) - ie. afconvert -f caff -d aacl@22050 -c 1 sound.aiff soundFileName.caf 

#### UWP
* Supports aac, flac, m4a, mp3, wav, & wma file formats
* For desktop v1511, custom audio will not work.  The plugin will ignore the sound config if it sees this.
* Read the following article for more info: https://blogs.msdn.microsoft.com/tiles_and_toasts/2016/06/18/quickstart-sending-a-toast-notification-with-custom-audio/
