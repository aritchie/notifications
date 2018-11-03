using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Notifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Essentials;


namespace Samples
{
    public class LocationTriggerViewModel : ViewModel
    {
        const double DEFAULT_DISTANCE_METERS = 200;
        readonly INotificationManager notificationManager;
        readonly IUserDialogs dialogs;


        public LocationTriggerViewModel(INotificationManager notificationManager, IUserDialogs dialogs)
        {
            this.notificationManager = notificationManager;
            this.dialogs = dialogs;

            this.SetCurrentLocation = ReactiveCommand.CreateFromTask(async _ =>
            {
                var pos = await Geolocation.GetLastKnownLocationAsync();
                if (pos == null)
                    dialogs.Alert("Could not get current position");
                else
                {
                    this.Identifier = $"{pos.Latitude}_{pos.Longitude}";
                    this.CenterLatitude = pos.Latitude;
                    this.CenterLongitude = pos.Longitude;
                }
            });

            this.CreateGeofence = ReactiveCommand.CreateFromTask(
                async _ =>
                {
                    await this.AddGeofence(
                        "YOUR LOCATION TEST",
                        "Hello",
                        this.CenterLatitude,
                        this.CenterLongitude,
                        this.RadiusMeters
                    );
                    this.Identifier = String.Empty;
                    this.CenterLatitude = 0;
                    this.CenterLongitude = 0;
                    this.RadiusMeters = 200;
                },
                this.WhenAny(
                    x => x.Identifier,
                    x => x.RadiusMeters,
                    x => x.CenterLatitude,
                    x => x.CenterLongitude,
                    x => x.NotifyOnEntry,
                    x => x.NotifyOnExit,
                    (id, rad, lat, lng, entry, exit) =>
                    {
                        if (String.IsNullOrWhiteSpace(id.GetValue()))
                            return false;

                        var radius = rad.GetValue();
                        if (radius < 200 || radius > 5000)
                            return false;

                        var latv = lat.GetValue();
                        if (latv >= 89.9 || latv <= -89.9)
                            return false;

                        var lngv = lng.GetValue();
                        if (lngv >= 179.9 || lngv <= -179.9)
                            return false;

                        if (!entry.GetValue() && !exit.GetValue())
                            return false;

                        return true;
                    }
                ));

            var hasEventType = this.WhenAny(
                x => x.NotifyOnEntry,
                x => x.NotifyOnExit,
                (entry, exit) => entry.GetValue() || entry.GetValue()
            );

            this.AddCnTower = ReactiveCommand.CreateFromTask(
                _ => this.AddGeofence(
                    "CN Tower - Toronto",
                    "Don't look down",
                    43.6425662,
                    -79.3892508,
                    DEFAULT_DISTANCE_METERS
                ),
                hasEventType
            );

            this.AddAppleHQ = ReactiveCommand.CreateFromTask(
                _ => this.AddGeofence(
                    "AppleHQ",
                    "Say Hi or Goodbye to Tim",
                    37.3320045,
                    -122.0329699,
                    DEFAULT_DISTANCE_METERS
                ),
                hasEventType
            );
        }

        public ICommand CreateGeofence { get; }
        public ICommand SetCurrentLocation { get; }

        public ICommand AddCnTower { get; }
        public ICommand AddAppleHQ { get; }

        [Reactive] public string Identifier { get; set; }
        [Reactive] public double CenterLatitude { get; set; }
        [Reactive] public double CenterLongitude { get; set; }
        [Reactive] public double RadiusMeters { get; set; } = 200;
        [Reactive] public bool SingleUse { get; set; }
        [Reactive] public bool NotifyOnEntry { get; set; } = true;
        [Reactive] public bool NotifyOnExit { get; set; } = true;


        async Task AddGeofence(string title, string message, double lat, double lng, double distance)
        {
            await this.notificationManager.Send(new NotificationRequest
            {
                Title = title,
                Message = message,
                Trigger = new LocationNotificationTrigger(distance, lat, lng, !this.SingleUse)
                {
                    NotifyOnExit = this.NotifyOnExit,
                    NotifyOnEntry = this.NotifyOnEntry
                }
            });
            this.dialogs.Toast("Geofence Notification Created");
        }
    }
}
