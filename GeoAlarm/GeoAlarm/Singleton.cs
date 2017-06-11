using GeoAlarm.Layouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoAlarm
{
    public class Singleton
    {
        private static Singleton instance;
        public MapXamlPage mapPage { get; set; }
        public States state { get; set; }

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
