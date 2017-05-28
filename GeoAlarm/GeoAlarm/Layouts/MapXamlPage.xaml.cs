using System;
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
        private readonly int CAMERA_LATITUDE_OFFSET = 80000;
        private readonly int CAMERA_ZOOM_OFFSET = 1000;
        private readonly uint ANIMATION_SPEED = 400;

        public MapXamlPage ()
		{
            InitializeComponent ();

            //Testing 
            CustomMap customMap = (CustomMap) geoAlarmMap;
            customMap.MoveToRegion(new MapSpan(new Position(100, 100), 360, 360));
            List<CustomPin> cPins = TestData.getTestCustomPins();
            customMap.CustomPins = cPins;
            closeAlarmMenu();

        }

        void OnSliderValueChanged(object sender,
                                  ValueChangedEventArgs args)
        {
            var text = "Area Size : " + ((Slider)sender).Value.ToString("F3");
            sliderLabel.Text = text;
        }

        void OnButtonClicked(object sender, EventArgs args)
        {
            closeAlarmMenu();
            saveAlarm();
        }


        public void closeAlarmMenu()
        {
            editMenu.TranslateTo(0, 1000, ANIMATION_SPEED);
        }

        public void openAlarmMenu()
        {
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
        }
    }
}
