using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GeoAlarm
{
    public class CustomPin
    {
        public enum IconType { Green, Red, Blue};
        public Pin Pin { get; set; }
        public string Id { get; set; }
        public Alarm Alarm { get; set; }
        public IconType Icon { get; set; }
    }
}
