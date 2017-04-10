﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Platform.Android;

namespace GeoAlarm
{
    public static class TestData
    {
        public static List<CustomPin> getTestCustomPins()
        {
            // For testing only


            Alarm myAlarm1 = new Alarm
            {
                Name = "myAlarm1",
                Radius = 1000,
                Active = false,
                ActiveDays = new DayOfWeek[] { DayOfWeek.Sunday },
                AlarmType = Alarm.Type.Single,
                StartTime = "18:00"
            };



            Alarm myAlarm2 = new Alarm
            {
                Name = "myAlarm2",
                Radius = 3000,
                Active = false,
                ActiveDays = new DayOfWeek[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday },
                AlarmType = Alarm.Type.Repetitive,
                StartTime = "16:00"
            };

            Alarm myAlarm3 = new Alarm
            {
                Name = "myAlarm3",
                Radius = 5000,
                Active = true,
                ActiveDays = new DayOfWeek[] { DayOfWeek.Friday },
                AlarmType = Alarm.Type.Single,
                StartTime = "15:00"
            };


            var pin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(37.79752, -122.40183),
                    Label = myAlarm1.Name,
                    Address = "394 Pacific Ave, San Francisco CA"
                },
                Id = "Alarm",
                Alarm = myAlarm1,
                Icon = CustomPin.IconType.Green
            };

            var pin2 = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(37.78, -122.40183),
                    Label = myAlarm2.Name,
                    Address = "394 Pacific Ave, San Francisco CA"
                },
                Id = "Alarm",
                Alarm = myAlarm2,
                Icon = CustomPin.IconType.Red
            };

            var pin3 = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(37.77, -122.40183),
                    Label = myAlarm3.Name,
                    Address = "394 Pacific Ave, San Francisco CA"
                },
                Id = "Alarm",
                Alarm = myAlarm3,
                Icon = CustomPin.IconType.Green
            };

            return new List<CustomPin> { pin, pin2, pin3 };
        }
    }
}
