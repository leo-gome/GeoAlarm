using System;
using System.Collections.Generic;
using System.Text;

namespace GeoAlarm
{
    interface IAlarm
    {
        string name
        {
            get;
            set;
        }

        //ToDo: Get alternative to string
        string name
        {
            get;
            set;
        }

        float areaSize
        {
            get;
            set;
        }

        //ToDo: Get alternative to string
        string[] timeInterval
        {
            get;
            set;
        }

        bool[] weekDays
        {
            get;
            set;
        }
    }
}
