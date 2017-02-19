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
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(customMap);
            Content = stack;

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
                Url = "http://xamarin.com/about/"
            };

            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin.Pin);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(
              new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));

            var position = new Position(37.79752, -122.40183);
            customMap.Circle = new CustomCircle
            {
                Position = position,
                Radius = 1000
            };

            customMap.ShapeCoordinates.Add(new Position(37.797513, -122.402058));
            customMap.ShapeCoordinates.Add(new Position(37.798433, -122.402256));
            customMap.ShapeCoordinates.Add(new Position(37.798582, -122.401071));
            customMap.ShapeCoordinates.Add(new Position(37.797658, -122.400888));


        }

    }
}
