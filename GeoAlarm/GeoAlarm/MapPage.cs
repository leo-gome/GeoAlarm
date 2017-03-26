using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System.Collections.Generic;

namespace GeoAlarm
{
    public class MapPage : ContentPage
    {
        private CustomMap customMap;
        public CustomPin selectedPin { get; set; }

        private RelativeLayout stack;
        public bool alarmMenuShown = false;

        private readonly int MAX_RADIUS = 5000;
        private readonly int MIN_RADIUS = 500;

        public MapPage()
        {
            customMap = new CustomMap
            {
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            customMap.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));


            // For testing only

            Alarm myAlarm1 = new Alarm
            {
                Name = "myAlarm1",
                Radius = 1000
            };

            Alarm myAlarm2 = new Alarm
            {
                Name = "myAlarm2",
                Radius = 3000
            };

            Alarm myAlarm3 = new Alarm
            {
                Name = "myAlarm3",
                Radius = 5000
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
                Alarm = myAlarm1
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
                Alarm = myAlarm2
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
                Alarm = myAlarm3
            };




            List<CustomPin> cPins = new List<CustomPin> { pin, pin2, pin3 };
            customMap.CustomPins = cPins;
            foreach (CustomPin cp in cPins)
            {
                customMap.Pins.Add(cp.Pin);
            }
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
              pin.Pin.Position, Distance.FromMiles(1.0)));            
            
            // put the page together
            stack = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            stack.Children.Add(customMap,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));
            Content = stack;
        }
        
        public void openEditMenu()
        {
            drawAlarm();
            showAlarmMenu();
        }
        
        public void closeEditMenu()
        {
            if (stack.Children.Count >= 2)
            {
                stack.Children.RemoveAt(1);
                Content = stack;
                MessagingCenter.Send<MapPage, string>(this, "RedrawMe", "pinsOnly");
            }
        }

        private void showAlarmMenu()
        {
            
            var submitButton = new Button { Text = "Submit" };
            submitButton.Clicked += (sender, e) => {
                //changeMyContent();
                closeEditMenu();
                MessagingCenter.Send<MapPage, string>(this, "RedrawMe", "pinsOnly");

            };
            
            var alarmNameLabel = new Label { Text = "Alarm name" };
            var alarmNameEntry = new Entry { Text = selectedPin.Alarm.Name};
            var areaLabel = new Label { Text = "Area Size" };
            var areaSlider = new Slider { Maximum = MAX_RADIUS, Minimum = MIN_RADIUS, Value = selectedPin.Alarm.Radius };
            var startTimeLabel = new Label { Text = "Start" };
            var startTime = new TimePicker { Format = "h:mm tt"};
            var endTimeLabel = new Label { Text = "End" };
            var endTime = new TimePicker { Format = "h:mm tt" };
            areaSlider.ValueChanged += OnSliderValueChanged;

            var alarmLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                     alarmNameLabel,
                     alarmNameEntry,
                     areaLabel,
                     areaSlider,
                     startTimeLabel,
                     startTime,
                     endTimeLabel,
                     endTime,
                     submitButton
                },
                Padding = 15,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromRgba (0,0,0, 180)            
            };
            stack.Children.Add(alarmLayout,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.RelativeToParent((parent) => { return parent.Height / 2; }),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height / 2; }));
            Content = stack;
            
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            selectedPin.Alarm.Radius = e.NewValue;
            //double newDistance = e.NewValue / 1000;
            //double newLat = 37.787 - (newDistance / 1000);
            drawAlarm();
        }

        void drawAlarm()
        {
            MessagingCenter.Send<MapPage, string>(this, "RedrawMe", "all");
            var renderedLat = selectedPin.Pin.Position.Latitude - (selectedPin.Alarm.Radius / 80000);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
             new Position(renderedLat, selectedPin.Pin.Position.Longitude), Distance.FromMiles(selectedPin.Alarm.Radius / 1000)));
        }

       
    }

  
}
