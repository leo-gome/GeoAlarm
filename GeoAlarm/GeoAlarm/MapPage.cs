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
        
        public RelativeLayout relativeLayout;

        private readonly int MAX_RADIUS = 5000;
        private readonly int MIN_RADIUS = 500;

        // Tweakers
        private readonly int CAMERA_LATITUDE_OFFSET = 80000;
        private readonly int CAMERA_ZOOM_OFFSET = 1000;
        private readonly uint ANIMATION_SPEED = 400;

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
            List<CustomPin> cPins = TestData.getTestCustomPins();
            
            customMap.CustomPins = cPins;

            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
              cPins[0].Pin.Position, Distance.FromMiles(1.0)));
            selectedPin = cPins[0];
            // put the page together
            relativeLayout = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
            };

            searchBar.SearchCommand = new Command(async () =>
            {
                var searchGeocoder = new Geocoder();
                var possibleAddresses = (await searchGeocoder.GetPositionsForAddressAsync(searchBar.Text));
                foreach (var a in possibleAddresses)
                {
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                     new Position(a.Latitude, a.Longitude), Distance.FromMiles(selectedPin.Alarm.Radius / CAMERA_ZOOM_OFFSET)));
                }
            });

            var searchbarLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(-10, 0, 0, 0),
                BackgroundColor = Color.FromRgba(0, 0, 0, 180)
            };
            searchbarLayout.Children.Add(searchBar);

            relativeLayout.Children.Add(customMap,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));


            createAlarmMenu();
            closeAlarmMenu();


            relativeLayout.Children.Add(searchbarLayout,
                    xConstraint: Constraint.Constant(10),
                    yConstraint: Constraint.Constant(10),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width - 20; }),
                    heightConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height / 16) + 10; }));

            Content = relativeLayout;
        }
         
        public void openAlarmMenu()
        {
            // Get Alarm Layout
            var alarmLayout = (StackLayout) relativeLayout.Children[1];

            // Get Children from Alarm Layout
            var alarmNameEntry = (Entry) alarmLayout.Children[1];
            alarmNameEntry.Text = selectedPin.Alarm.Name;

            var areaSlider = (Slider)alarmLayout.Children[3];
            areaSlider.Value = selectedPin.Alarm.Radius;

            var startTime = (TimePicker)alarmLayout.Children[5];
            startTime.Time = selectedPin.Alarm.StartTime;

            var endTime = (TimePicker)alarmLayout.Children[7];
            endTime.Time = selectedPin.Alarm.EndTime;

            drawAlarm();
            relativeLayout.Children[1].TranslateTo(0, 0, ANIMATION_SPEED);
        }
        
        public void closeAlarmMenu()
        {
            relativeLayout.Children[1].TranslateTo(0, 1000, ANIMATION_SPEED);         
        }
        
        /*
         * Creates the alarm menu.
         * The menu is a Stacklayout which is added to the main layout of the map
         */        
        private void createAlarmMenu()
        {            
            var submitButton = new Button { Text = "Submit" };
            submitButton.Clicked += (sender, e) => {
                closeAlarmMenu();
                cleanAlarmCircle();
                saveAlarm();
                reCenterMap();
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
            relativeLayout.Children.Add(alarmLayout,
                    xConstraint: Constraint.Constant(0),
                    yConstraint: Constraint.RelativeToParent((parent) => { return parent.Height / 2; }),
                    widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height / 2; }));
            Content = relativeLayout;            
        }

        /*
         * Changes the radius of the area when the slider changes its value
         */ 
        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            selectedPin.Alarm.Radius = e.NewValue;
            drawAlarm();
        }

        /*
         * This function draw all elements on map. (Pins, Circle)
         * It also moves the camera to the center of selected pin
         */
        void drawAlarm()
        {
            MessagingCenter.Send<MapPage, string>(this, "RedrawMe", "all");
            var renderedLat = selectedPin.Pin.Position.Latitude - (selectedPin.Alarm.Radius / CAMERA_LATITUDE_OFFSET);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
             new Position(renderedLat, selectedPin.Pin.Position.Longitude), Distance.FromMiles(selectedPin.Alarm.Radius / CAMERA_ZOOM_OFFSET)));
        }

        /*
         * Clears the map and redraws only the pins. 
         */ 
        void cleanAlarmCircle()
        {
            MessagingCenter.Send<MapPage, string>(this, "RedrawMe", "pinsOnly");
        }

        /*
         * This function read the inputs on the menu and save its in the alarm 
         * attached to the selected pin
         */
        void saveAlarm()
        {
            selectedPin.PinType = "Alarm";
            selectedPin.Icon = CustomPin.IconType.Green;

            // Get Alarm Layout
            var alarmLayout = (StackLayout)relativeLayout.Children[1];

            // Get Children from Alarm Layout
            var alarmNameEntry = (Entry)alarmLayout.Children[1];
            selectedPin.Alarm.Name = alarmNameEntry.Text;

            var areaSlider = (Slider)alarmLayout.Children[3];
            selectedPin.Alarm.Radius = areaSlider.Value;

            var startTime = (TimePicker)alarmLayout.Children[5];
            selectedPin.Alarm.StartTime = startTime.Time;

            var endTime = (TimePicker)alarmLayout.Children[7];
            selectedPin.Alarm.EndTime = endTime.Time;
            
            MessagingCenter.Send<MapPage, string>(this, "Save", "Alarm");
        }

        /*
         * Camera is focus on the selected pin
         */ 
        public void reCenterMap()
        {
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
             selectedPin.Pin.Position, Distance.FromMiles(selectedPin.Alarm.Radius / CAMERA_ZOOM_OFFSET)));
        }
    }

  
}
