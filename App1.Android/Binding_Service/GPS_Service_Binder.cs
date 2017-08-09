using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;

namespace App1.Droid
{
    public class GPS_Service_Binder : Binder
    {
        public GPS_Service_Binder(GPS_Service_Android service)
        {
            this.Service = service;
        }

        public GPS_Service_Android Service { get; private set; }

    }
}