using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GeoAlarm
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
        public CustomCircle Circle { get; set; }
        public List<Position> ShapeCoordinates { get; set; }

        public CustomMap()
        {
            ShapeCoordinates = new List<Position>();
        }


    }
}
