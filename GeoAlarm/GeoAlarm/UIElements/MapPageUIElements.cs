using GeoAlarm.Tweakers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GeoAlarm.MenuElements
{
    public class MapPageUIElements
    {
        public static CustomMap customMap = new CustomMap
            {
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
        };

        public static Button submitButton = new Button { Text = "Submit" };

        public static SearchBar searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
            };

        public static StackLayout searchbarLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Padding = new Thickness(-10, 0, 0, 0),
            BackgroundColor = Color.FromRgba(0, 0, 0, 180)
        };

        public static StackLayout alarmLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Children =
            {
                new Label { Text = "Alarm name" },
                new Entry { Text = ""},
                new Label { Text = "Area Size" },
                new Slider { Maximum = TweakersMapPage.MAX_RADIUS, Minimum = TweakersMapPage.MIN_RADIUS, Value = 0},
                new Label { Text = "Start" },
                new TimePicker { Format = "h:mm tt" },
                new Label { Text = "End" },
                new TimePicker { Format = "h:mm tt" }
            },

            Padding = 15,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            BackgroundColor = Color.FromRgba(0, 0, 0, 180)
        };
    }
}
