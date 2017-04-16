using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GeoAlarm
{
    public class Alarm
    {
        public enum Type { Single, Repetitive };

        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public double Radius { get; set; }
        public bool Active { get; set; }
        public DayOfWeek[] ActiveDays { get; set; }
        public Type AlarmType { get; set; }

        public string getActiveDaysInStr()
        {
            if (ActiveDays.Length == 1)
            {
                return ActiveDays[0].ToString();
            }
            else if (ActiveDays.Length >= 2)
            {
                return ActiveDays[0].ToString() + " - " + ActiveDays[ActiveDays.Length - 1].ToString();
            }
            return "-";
        }
    }
}
