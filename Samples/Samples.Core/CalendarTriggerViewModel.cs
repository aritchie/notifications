using System;
using System.Windows.Input;
using ReactiveUI.Fody.Helpers;


namespace Samples
{
    public class CalendarTriggerViewModel : ViewModel
    {
        public CalendarTriggerViewModel()
        {
        }
        
        
        public ICommand Create { get; }
        [Reactive] public DateTime Date { get; set; }
        [Reactive] public TimeSpan Time { get; set; }
    }
}
