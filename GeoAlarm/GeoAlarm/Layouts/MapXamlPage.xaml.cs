﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoAlarm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoAlarm.Layouts
{
	public partial class MapXamlPage : ContentPage
    {
        private CustomMap customMap;
        public CustomPin selectedPin { get; set; }

        private readonly int MAX_RADIUS = 5000;
        private readonly int MIN_RADIUS = 500;

        // Tweakers
        private readonly int CAMERA_LATITUDE_OFFSET = 90000;
        private readonly int CAMERA_ZOOM_OFFSET = 1000;
        private readonly uint ANIMATION_SPEED = 400;

        public MapXamlPage ()
		{
            InitializeComponent ();
            slider.Maximum = MAX_RADIUS;
            slider.Minimum = MIN_RADIUS;

            // Init State
            Singleton.Instance.state = States.STANDARD;

            //Testing 
            this.customMap = (CustomMap) geoAlarmMap;
            List<CustomPin> cPins = TestData.getTestCustomPins();
            customMap.CustomPins = cPins;
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
              cPins[0].Pin.Position, Distance.FromMiles(1.0)));
            selectedPin = cPins[0];
            closeAlarmMenu();



           // autoComplete.ValueChanged += updateSugestions;
        }

        void OnSliderValueChanged(object sender,
                                  ValueChangedEventArgs args)
        {
            if (selectedPin != null)
            {
                var text = "Area Size : " + ((Slider)sender).Value.ToString("F3");
                sliderLabel.Text = text;
                selectedPin.Alarm.Radius = args.NewValue;
                drawAlarm();
            }          
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            closeAlarmMenu();
            saveAlarm();
            cleanAlarmCircle();
            Singleton.Instance.state = States.STANDARD;
        }


        public void closeAlarmMenu()
        {
            editMenu.TranslateTo(0, 1000, ANIMATION_SPEED);
        }

        public void openAlarmMenu()
        {
            alarmName.Text = selectedPin.Alarm.Name;
            slider.Value = selectedPin.Alarm.Radius;
            startTime.Time = selectedPin.Alarm.StartTime;
            endTime.Time = selectedPin.Alarm.EndTime;
            drawAlarm();
            editMenu.TranslateTo(0, 0, ANIMATION_SPEED);
        }

        void saveAlarm()
        {
            selectedPin.PinType = "Alarm";
            selectedPin.Icon = CustomPin.IconType.Green;
            selectedPin.Alarm.Name = alarmName.Text;           
            selectedPin.Alarm.Radius = slider.Value;            
            selectedPin.Alarm.StartTime = startTime.Time;            
            selectedPin.Alarm.EndTime = endTime.Time;
            MessagingCenter.Send<MapXamlPage, string>(this, "Save", "Alarm");
            Singleton.Instance.state = States.STANDARD;
        }

        public void removeAlarm()
        {
            customMap.CustomPins.Remove(selectedPin);
            MessagingCenter.Send<MapXamlPage, string>(this, "RedrawMe", "pinsOnly");
            Singleton.Instance.state = States.STANDARD;

        }


        void drawAlarm()
        {
            MessagingCenter.Send<MapXamlPage, string>(this, "RedrawMe", "all");
            var renderedLat = selectedPin.Pin.Position.Latitude - (selectedPin.Alarm.Radius / CAMERA_LATITUDE_OFFSET);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
             new Position(renderedLat, selectedPin.Pin.Position.Longitude), Distance.FromMiles(selectedPin.Alarm.Radius / CAMERA_ZOOM_OFFSET)));
        }


        void cleanAlarmCircle()
        {
            MessagingCenter.Send<MapXamlPage, string>(this, "RedrawMe", "pinsOnly");
        }

        async void onSearchButtonClicked()
        {
            /*
            var searchGeocoder = new Geocoder();
            List<Position> possibleAddresses = (List<Position>)(await searchGeocoder.GetPositionsForAddressAsync(searchLocation.Text));
            if (possibleAddresses.Count > 0)
            {
                Position a = possibleAddresses[0];
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                 new Position(a.Latitude, a.Longitude), Distance.FromMiles(selectedPin.Alarm.Radius / CAMERA_ZOOM_OFFSET)));

            }
            foreach (var a in possibleAddresses)
            {
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                 new Position(a.Latitude, a.Longitude), Distance.FromMiles(selectedPin.Alarm.Radius / CAMERA_ZOOM_OFFSET)));
            }
            */
        }
        /*
        async void updateSugestions(object o, Syncfusion.SfAutoComplete.XForms.ValueChangedEventArgs args)
        {
            var searchGeocoder = new Geocoder();
            List<Object> possibleAddresses = (List<Object>)(await searchGeocoder.GetPositionsForAddressAsync(autoComplete.Text));
            autoComplete.DataSource = possibleAddresses;
        }
        */

    }
}
