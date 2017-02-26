using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GeoAlarm 
{
    class AlarmPage : ContentPage
    {
        public AlarmPage()
        {
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Label {
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "Welcome to Xamarin Forms2!"
                    }
                }
            };
        }
    }
}
