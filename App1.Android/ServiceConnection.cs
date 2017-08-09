using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;
//using Plugin.Geolocator;
using static App1.Droid.Servis;


namespace App1.Droid
{
    public class ServiceConnection : Java.Lang.Object, IServiceConnection
    {
        MainActivity ma;
        public ServiceConnection(MainActivity activity)
        {
            ma = activity;
        }


        /*public void Dispose()
        {
        }*/

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            LocalBinder binder = (LocalBinder)service;
            ma.mService = binder.GetService();
            ma.isConnected = true;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            ma.isConnected = false;
        }
    }
}

