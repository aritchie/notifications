using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Acr.Notifications
{
    class NotificationSettings : INotifyPropertyChanged
    {
        static readonly Lazy<NotificationSettings> instance = new Lazy<NotificationSettings>(
            () => Settings.Settings.Local.Bind<NotificationSettings>()
        );
        public static NotificationSettings Instance => instance.Value;
        readonly object syncLock = new object();

        public NotificationSettings()
        {
            this.ScheduleIds = new List<int>();
        }


        public List<int> ScheduleIds { get; set; }
        public int CurrentScheduleId { get; set; }
        public int CurrentBadge { get; set; }


        public int CreateScheduleId()
        {
            var id = 0;

            lock (this.syncLock)
            {
                this.CurrentScheduleId++;
            }
            this.ScheduleIds.Add(this.CurrentScheduleId);
            this.OnPropertyChanged("CurrentScheduleId");
            this.OnPropertyChanged("ScheduleIds");
            return id;
        }


        public void RemoveScheduledId(int id)
        {
            if (this.ScheduleIds.Remove(id))
                this.OnPropertyChanged("ScheduleIds");
        }


        public void ClearScheduled()
        {
            this.ScheduleIds.Clear();
            this.OnPropertyChanged("ScheduleIds");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}