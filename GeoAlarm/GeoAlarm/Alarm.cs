using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GeoAlarm
{
    public class Alarm
    {
        public string Name { get; set; }
        public TimePicker StartTime { get; set; }
        public TimePicker EndTime { get; set; }
        public double Radius { get; set; }
    }
}
