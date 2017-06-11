using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace GeoAlarm.Droid
{
	[Activity (Label = "GeoAlarm", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);
            //Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            //Window.SetStatusBarColor(GUI.ColorConfig.APP_COLOR_THEME.ToAndroid());
            LoadApplication (new GeoAlarm.App ());
		}
	}
}

