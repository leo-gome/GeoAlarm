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



[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace GeoAlarm.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback, GoogleMap.IOnInfoWindowClickListener
    {
        GoogleMap map;
        List<CustomPin> customPins;
        CustomCircle circle;
        List<Position> shapeCoordinates;

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

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.SetInfoWindowAdapter(this);
            map.SetOnInfoWindowClickListener(this);
            map.CameraChange += OnCameraChanged;
            map.Clear();
            reDrawPins();

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

            MessagingCenter.Subscribe<MapPage, string>(this, "Save", (sender, arg) =>
            {
                if (arg == "Alarm")
                {
                    // TODO: Persist Alarm
                    Toast.MakeText(this.Context, "TODO: Save me :)", ToastLength.Long).Show();
                }
            });
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
            }
        }
        
        void OnCameraChanged(object sender, GoogleMap.CameraChangeEventArgs e)
        {

        }
        

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

        void reDrawPins()
        {
            // Add pins
            foreach (var pin in customPins)
            {
                var marker = new MarkerOptions();
                marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                marker.SetTitle(pin.Pin.Label);
                marker.SetSnippet(pin.Pin.Address);
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
                map.AddMarker(marker);
            }
        }
        

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
                view = inflater.Inflate(Resource.Layout.AlarmInfo, null);
                
                App.myMapPage.selectedPin = customPin;

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

                return view;
            }
            

            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

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

        public void OnInfoWindowClick(Marker marker)
        {
            App.myMapPage.openAlarmMenu();
        }
    }
}