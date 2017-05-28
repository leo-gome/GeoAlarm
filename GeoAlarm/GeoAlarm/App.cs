using GeoAlarm.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GeoAlarm
{
	public class App : Application
	{
        //public static MapPage myMapPage;
        public App ()
		{
            Singleton.Instance.mapPage = new MapXamlPage();
            MainPage = Singleton.Instance.mapPage;

            //myMapPage = new MapPage();
            //MainPage = myMapPage;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
