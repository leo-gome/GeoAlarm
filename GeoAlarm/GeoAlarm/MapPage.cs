using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System.Collections.Generic;

namespace GeoAlarm
{
    public class MapPage : ContentPage
    {
        CustomMap customMap;
        StackLayout stack;
        bool alarmMenuShown = false;
        public bool redraw = false;
        public MapPage()
        {
            customMap = new CustomMap
            {
                //IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            // You can use MapSpan.FromCenterAndRadius 
            //map.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37, -122), Distance.FromMiles (0.3)));
            // or create a new MapSpan object directly
            customMap.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));
               

            // put the page together
            /*
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(customMap);
            Content = stack;
            */

            var pin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(37.79752, -122.40183),
                    Label = "Xamarin San Francisco Office",
                    Address = "394 Pacific Ave, San Francisco CA"
                },
                Id = "Xamarin",
                Url = "http://www.google.com"
            };

            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin.Pin);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));

            var position = new Position(37.79752, -122.40183);
            customMap.Circle = new CustomCircle
            {
                Position = position,
                Radius = 1000,
                Url = "http://www.google.com"
            };

            customMap.ShapeCoordinates.Add(new Position(37.797513, -122.402058));
            customMap.ShapeCoordinates.Add(new Position(37.798433, -122.402256));
            customMap.ShapeCoordinates.Add(new Position(37.798582, -122.401071));
            customMap.ShapeCoordinates.Add(new Position(37.797658, -122.400888));

            /*
            // create buttons
            var morePins = new Button { Text = "Add more pins" };
            morePins.Clicked += (sender, e) => {
                customMap.Pins.Add(new Pin
                {
                    Position = new Position(36.9641949, -122.0177232),
                    Label = "Boardwalk"
                });
                customMap.Pins.Add(new Pin
                {
                    Position = new Position(36.9571571, -122.0173544),
                    Label = "Wharf"
                });
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(36.9628066, -122.0194722), Distance.FromMiles(1.5)));

            };
            var reLocate = new Button { Text = "Re-center" };
            reLocate.Clicked += (sender, e) => {
                stack.Children.RemoveAt(1);
                Content = stack;
            };
            var buttons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    morePins, reLocate
                }
            };
            */

            // put the page together
            stack = new StackLayout
            {
                Spacing = 0
            };

            stack.Children.Add(customMap);
            Content = stack;

        }

        public void changeMyContent()
        {
            if (alarmMenuShown)
            {
                stack.Children.RemoveAt(1);
                Content = stack;
            }
            else
            {
                showAlarmMenu();
            }
            alarmMenuShown = !alarmMenuShown;
        }

        private void showAlarmMenu()
        {
            var submitButton = new Button { Text = "Submit" };
            submitButton.Clicked += (sender, e) => {
                changeMyContent();
            };
            
            var alarmNameEntry = new Entry { Text = "I am an Entry" };
            var alarmNameLabel = new Label { Text = "Alarm name" };
            var areaLabel = new Label { Text = "Area Size" };
            var areaSlider = new Slider { Maximum = 5000, Minimum = 100, Value = customMap.Circle.Radius };
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
                }
            };
            stack.Children.Add(alarmLayout);
            Content = stack;
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            customMap.Circle.Radius = e.NewValue;
            double newDistance = e.NewValue / 1000;
            redraw = true;
            MessagingCenter.Send<MapPage>(this, "RedrawMe");
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
             new Position(37.79752, -122.40183), Distance.FromMiles(newDistance)));

        }

       
    }

  
}
