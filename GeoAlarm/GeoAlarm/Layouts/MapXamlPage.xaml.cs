using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoAlarm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoAlarm.Layouts
{
	public partial class MapXamlPage : ContentPage
	{
		public MapXamlPage ()
		{
            InitializeComponent ();
            CustomMap customMap = (CustomMap) geoAlarmMap;
            customMap.MoveToRegion(new MapSpan(new Position(100, 100), 360, 360));

        }

        void OnSliderValueChanged(object sender,
                                  ValueChangedEventArgs args)
        {
            var text = "Area Size : " + ((Slider)sender).Value.ToString("F3");
            valueLabel.Text = text;
        }

        void OnButtonClicked(object sender, EventArgs args)
        {

        }
    }
}
