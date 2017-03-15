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
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        GoogleMap map;
        List<CustomPin> customPins;
        CustomCircle circle;
        List<Position> shapeCoordinates;
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                map.InfoWindowClick -= OnInfoWindowClick;
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
            map.InfoWindowClick += OnInfoWindowClick;
            map.SetInfoWindowAdapter(this);
            map.CameraChange += OnCameraChanged;

            MessagingCenter.Subscribe<MapPage>(this, "RedrawMe", (sender) => {
                map.Clear();
                reDrawCircle();
                reDrawPin();
            });

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion") && !isDrawn)
            {
                map.Clear();
                isDrawn = true;
            }            
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
                isDrawn = false;
            }
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            App.myMapPage.changeMyContent();
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

        void reDrawPin()
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
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                if (customPin.Id == "Alarm")
                {
                    App.myMapPage.selectedPin = customPin;
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                }

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }

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
        
    }
}