using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.Maps;

using System.Diagnostics;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Text.RegularExpressions;

using System.Threading;

namespace App1.Droid
{
    [Service(Name = "com.xamarin.ServicesDemo1")]
    public class GPS_Service_Android : global::Android.App.Service//, IGetTimestamp
    {
        static readonly int highAccuracy = 10;//, lowAccuracy = 500;
        //Stopwatch sw = new Stopwatch();

        static readonly string TAG = typeof(GPS_Service_Android).FullName;
        public static double latitude, longtitude, speed, altitude;
        //static double prevlat, prevlong;
        public static DateTimeOffset timestamp;
        //bool listening = false;
        Thread thread;
        Map mapref;
        //IGetTimestamp timestamper;

        Wifi_coarse lowController;

        public App1.Sensors.TipZemljevida tz;

        //public EventTrigger et;
        //Threadevent te;


        public IBinder Binder { get; private set; }

        public override void OnCreate()
        {
            // This method is optional to implement
            base.OnCreate();
            Log.Debug(TAG, "OnCreate");

            

            mapref = Sensors.MapsPage.mapRef;
            lowController = new Wifi_coarse(this);

            //et = new EventTrigger();
            //te = new Threadevent(et, this);

            lowController.Start();
            //startTrack();

            tz = new Sensors.TipZemljevida();
            tz.attach(tz);



            //Intent intent = new Intent(this, typeof(Wifi_coarse));
            //StartActivity(intent);
            /*var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);

            var timer = new System.Threading.Timer((e) =>
            {
                track();
            }, null, startTimeSpan, periodTimeSpan);
            //timestamper = new UtcTimestamper();*/
        }

        public override IBinder OnBind(Intent intent)
        {
            // This method must always be implemented
            Log.Debug(TAG, "OnBind");
            this.Binder = new GPS_Service_Binder(this);
            return this.Binder;
        }

        public override bool OnUnbind(Intent intent)
        {
            // This method is optional to implement
            Log.Debug(TAG, "OnUnbind");
            return base.OnUnbind(intent);
        }

        public override void OnDestroy()
        {
            // This method is optional to implement
            Log.Debug(TAG, "OnDestroy");
            Binder = null;
            //timestamper = null;
            base.OnDestroy();
        }


        public async void StartTrack()
        {


            var hasPermission = await Utils.CheckPermissions(Permission.Location);
            if (!hasPermission)
                return;

            CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = highAccuracy;

            tz.spremenivrednost("GPS");

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(10), 5,
                    true, null);
            //listening = true;

            /*void CrossGeolocator_Current_PositionError(object sender, PositionErrorEventArgs e)
            {

                labelGPSTrack.Text = "Location error: " + e.Error.ToString();
            }*/

        }

        void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                var position = e.Position;

                latitude = position.Latitude;
                longtitude = position.Longitude;
                speed = position.Speed;
                timestamp = position.Timestamp;

                UpdateMaps(position);

            });

            //if (sw.IsRunning)
            //{
            //    sw.Stop();
            //}
            //sw.Start();
            //CrossGeolocator.Current.DesiredAccuracy = highAccuracy;
            //App1.Sensors.MapsPage._theVariable = highAccuracy; //test


            if (thread != null)
            {
                thread.Abort();
            }
            thread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(11000);
                //et.callEvent();
                Looper.Prepare();
                TimeoutThread();
                Device.BeginInvokeOnMainThread(() =>
                {
                    tz.spremenivrednost("Omrežje");
                });
                
            });
            thread.Start();
        }

        /*void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
            var position = e.Position;

                latitude = position.Latitude;
                longtitude = position.Longitude;
                speed = position.Speed;
                timestamp = position.Timestamp;

            });
        }*/

        public void TimeoutThread()
        {
            CrossGeolocator.Current.StopListeningAsync();
            lowController.Start();
        }

        void UpdateMaps(Plugin.Geolocator.Abstractions.Position position)
        {
            mapref.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
        }

    }

    /*class Threadevent: EventListener
    {
        GPS_Service_Android gsa;
        public Threadevent(EventTrigger evt, GPS_Service_Android gsa) : base(evt)
        {
            this.gsa = gsa;
        }

        public override void evtTrigger(object sender, EventArgs e)
        {
            gsa.TimeoutThread();
        }
    }*/
}
