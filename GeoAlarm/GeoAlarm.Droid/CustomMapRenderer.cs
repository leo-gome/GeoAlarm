using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using GeoAlarm;
using GeoAlarm.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Android.App;
using System.Linq;



[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GeoAlarm.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback, GoogleMap.IOnInfoWindowClickListener
    {
        GoogleMap map;
        List<CustomPin> customPins;
        CustomCircle circle;
        List<Position> shapeCoordinates;
        bool shouldBeDelete = false;
        CustomPin oldPin = new CustomPin();

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
                circle = formsMap.Circle;
                shapeCoordinates = formsMap.ShapeCoordinates;
                ((MapView)Control).GetMapAsync(this);
            }            
        }

        /*
         * This function is called when the map is loaded for the first tinme.
         * Listener functions are added at this point.
         */ 
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.SetInfoWindowAdapter(this);
            map.SetOnInfoWindowClickListener(this);
            map.CameraChange += OnCameraChanged;
            map.MapClick += onMapClick;
            map.Clear();
            reDrawPins();

            // RedrawMe: Is used when the main thread needs to redraw any specific objects on the map
            // pinsOnly: Only the pins are drawn
            // all: Pins and the circle are drawn
            MessagingCenter.Subscribe<MapPage, string>(this, "RedrawMe", (sender, arg) =>
            {
                map.Clear();
                if (arg == "pinsOnly")
                {
                    reDrawPins();
                }
                else if (arg == "all")
                {
                    reDrawPins();
                    reDrawCircle();
                }
            });

            // Save: Is called in order to give five a feedback to the user that the alarm was saved
            MessagingCenter.Subscribe<MapPage, string>(this, "Save", (sender, arg) =>
            {
                if (arg == "Alarm")
                {
                    Toast.MakeText(this.Context, "TODO: Persist the alarm", ToastLength.Long).Show();
                    reDrawPins();
                }
            });
        }

        /*
         *  Called everytime the user clicks on map.
         *  Currently adds a custom pin on the map.
         */ 
        protected void onMapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            var strToShow = "Lat " +  e.Point.Latitude + "  long " + e.Point.Longitude;
            Toast.MakeText(this.Context, strToShow, ToastLength.Long).Show();
            createPin(e);
        }

        /*
         * Creates a new customPin on the map. 
         * The pin is treated as a temporal alarm and can be changed to an alarm
         */ 
        private void createPin(GoogleMap.MapClickEventArgs e)
        {
            Alarm myAlarm1 = new Alarm
            {
                Name = "TempAlarm",
                Radius = 1000,
                Active = false,
                ActiveDays = new DayOfWeek[] { DayOfWeek.Sunday },
                AlarmType = Alarm.Type.Single,
                StartTime = new TimeSpan(12,0,0)
            };

            var tempCustomPin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(e.Point.Latitude, e.Point.Longitude),
                    Label = "",
                    Address = ""
                },
                PinType = "TempAlarm",
                Alarm = myAlarm1,
                Icon = CustomPin.IconType.Blue
            };
            var lastCPin = customPins.Last();
            if(lastCPin.PinType == "TempAlarm")
            {
                customPins[customPins.Count - 1] = tempCustomPin;
            }
            else
            {
                customPins.Add(tempCustomPin);
            }
            oldPin = tempCustomPin;
            map.Clear();
            reDrawPins();
        }

        /*
         * Function not used at the moment
         */ 
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
        }

        /*
         * Function not used at the moment
         */
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
            }
        }

        /*
         * Function not used at the moment
         */
        void OnCameraChanged(object sender, GoogleMap.CameraChangeEventArgs e)
        {

        }
        
        /*
         * Draws circle on map with the selected Pin position and radius
         */ 
        void reDrawCircle()
        {
            // Add Circle
            var circleOptions = new CircleOptions();
            circleOptions.InvokeCenter(new LatLng(App.myMapPage.selectedPin.Pin.Position.Latitude, 
                App.myMapPage.selectedPin.Pin.Position.Longitude));
            circleOptions.InvokeRadius(App.myMapPage.selectedPin.Alarm.Radius);
            circleOptions.InvokeFillColor(0X66FF0000);
            circleOptions.InvokeStrokeColor(0X66FF0000);
            circleOptions.InvokeStrokeWidth(0);
            map.AddCircle(circleOptions);
        }

        /*
         * Draws all the pins on the map.
         */ 
        void reDrawPins()
        {
            // Add pins
            foreach (var pin in customPins)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                marker.SetTitle(pin.Pin.Label);
                marker.SetSnippet(pin.Pin.Address);
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Utils.PinUtils.getPinIdFromIconType(pin.Icon)));
                map.AddMarker(marker);
            }
        }
        
        /*
         * This function is called everytime a pin is click on the map.
         * It handles which kind of info message should pop on top of the pin.
         */ 
        public Android.Views.View GetInfoContents(Marker marker)
        {
            // Close menu
            App.myMapPage.closeAlarmMenu();

            // Inflate custom info screen       
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;
                CustomPin customPin = GetCustomPin(marker);                
                App.myMapPage.selectedPin = customPin;
                if (customPin.PinType == "Alarm")
                {
                    if (oldPin.Equals(customPin) && shouldBeDelete)
                    {
                        view = inflater.Inflate(Resource.Layout.CreateAlarmInfo, null);
                        var alarmTitle = view.FindViewById<TextView>(Resource.Id.AlarmTitle);
                        alarmTitle.Text = "Delete alarm";
                        shouldBeDelete = false;
                    }
                    else
                    {
                        view = inflater.Inflate(Resource.Layout.AlarmInfo, null);
                        var alarmTitle = view.FindViewById<TextView>(Resource.Id.AlarmTitle);
                        var alarmDays = view.FindViewById<TextView>(Resource.Id.AlarmDays);
                        var alarmTime = view.FindViewById<TextView>(Resource.Id.AlarmTime);
                        var alarmType = view.FindViewById<TextView>(Resource.Id.AlarmType);
                        var alarmRange = view.FindViewById<TextView>(Resource.Id.AlarmRange);
                        alarmTitle.Text = customPin.Alarm.Name;
                        alarmDays.Text = LanguageUtils.LanguageVariables.INFOSCREEN_DAYS + " : " + customPin.Alarm.getActiveDaysInStr();
                        alarmTime.Text = LanguageUtils.LanguageVariables.INFOSCREEN_TIME + " : " + customPin.Alarm.StartTime;
                        alarmType.Text = LanguageUtils.LanguageVariables.INFOSCREEN_TYPE + " : " + customPin.Alarm.AlarmType.ToString();
                        alarmRange.Text = LanguageUtils.LanguageVariables.INFOSCREEN_RANGE + " : " + customPin.Alarm.Radius.ToString();
                        shouldBeDelete = true;
                    }

                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.CreateAlarmInfo, null);
                    var alarmTitle = view.FindViewById<TextView>(Resource.Id.AlarmTitle);
                    alarmTitle.Text = "Create new alarm";
                }
                oldPin = customPin;
                return view;
            }
            return null;
        }

        /*
         * Function not used at the moment
         */             
        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        /*
         *  Gets a customPin from a marker.
         *  A marker is given from Google when the user clicks a pin on the map
         */ 
        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        /*
         * This function is called when the user click on the info screen on top of the pin
         */ 
        public void OnInfoWindowClick(Marker marker)
        {
            App.myMapPage.openAlarmMenu();
        }
    }
}