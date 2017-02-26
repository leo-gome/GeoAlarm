using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GeoAlarm
{
	public class App : Application
	{
        public static NavigationPage MyNavigationPage;
        public static MapPage myMapPage;
        public App ()
		{
            //MyNavigationPage = new NavigationPage(new MapPage());            
            myMapPage = new MapPage();
            MainPage = myMapPage;
            /*
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};
            */
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
