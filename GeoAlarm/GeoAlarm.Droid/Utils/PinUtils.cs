using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GeoAlarm.Droid.Utils
{
    public static class PinUtils
    {
        public static int getPinIdFromIconType(CustomPin.IconType type)
        {
            if (type == CustomPin.IconType.Red)
            {
                return Resource.Drawable.PinRed;
            }

            if (type == CustomPin.IconType.Green)
            {
                return Resource.Drawable.PinGreen;
            }

            if (type == CustomPin.IconType.Blue)
            {
                return Resource.Drawable.PinBlue;
            }

            return -1;
        }
    }
}