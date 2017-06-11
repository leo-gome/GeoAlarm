using GeoAlarm.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GeoAlarm
{
    class TabbedPageDemoPage : TabbedPage
    {
        public TabbedPageDemoPage()
        {

            var navigationPage = Singleton.Instance.mapPage;
            navigationPage.Title = "Map";
            navigationPage.Icon = "pin.png";

            var testPage = new NamedColorPage();
            testPage.Title = "Settings";

            Children.Add(navigationPage);
            Children.Add(testPage);
            
        }
    }

    // Data type:
    class NamedColor
    {
        public NamedColor(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
        }

        public string Name { private set; get; }

        public Color Color { private set; get; }

        public override string ToString()
        {
            return Name;
        }
    }

    // Format page
    class NamedColorPage : ContentPage
    {
        public NamedColorPage()
        {
            // This binding is necessary to label the tabs in
            // the TabbedPage.
            this.SetBinding(ContentPage.TitleProperty, "Name");
            // BoxView to show the color.
            BoxView boxView = new BoxView
            {
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center
            };
            boxView.SetBinding(BoxView.ColorProperty, "Color");

            // Build the page
            this.Content = boxView;
        }
    }
}
