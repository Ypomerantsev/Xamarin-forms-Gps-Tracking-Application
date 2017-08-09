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
using Android.Locations;
using App1.PodatkovnaBaza.Modeli;


namespace App1.Droid
{
    [Activity(Label = "Get Location", Icon = "@drawable/icon")]
    public class Wifi_coarse : Java.Lang.Object, ILocationListener
    {
        //static readonly string TAG = "X:" + typeof(Activity1).Name;
        //TextView _addressText;
        //Location _currentLocation;
        static LocationManager _locationManager;

        static string _locationProvider;
        //TextView _locationText;

        GPS_Service_Android parent;

        Servis stars;

        public Wifi_coarse(Servis part)
        {
            InitializeLocationManager();
            stars = part;
        }

        public Wifi_coarse(GPS_Service_Android gsa)
        {

            InitializeLocationManager();
            parent = gsa;
        }

        public Wifi_coarse()
        {
            parent = null;
        }


        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
            /*Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Coarse
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                foreach(String s in acceptableLocationProviders)
                {
                    if (s.Equals("network"))
                        _locationProvider = s;
                }
                //_locationProvider = "NETWORK_PROVIDER";
            }*/

            _locationProvider = LocationManager.NetworkProvider; ;
            

            /*if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }*/
            Console.WriteLine("Using " + _locationProvider + ".");
        }

        

        public void Start()
        {
            try
            {
                if(_locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
                    _locationManager.RequestLocationUpdates(_locationProvider, 2000, 1, this);

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);

            }
        }

        public void Stop()
        {
            _locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location) {
            //_locationManager.RemoveUpdates(this);

            stars.StartTrack();
            if (Sensors.OstaliSenzorji.coarse_switch)
            {
                GpsPodatek gp = new GpsPodatek();
                gp.NutshellSet(location.Time,location.Latitude,location.Longitude,location.Speed,0,location.Accuracy,location.Altitude,DateTimeOffset.Now,GpsPodatek.SwitchType.Podatek);
                stars.Database.SaveItemAsync(gp);
                CommunicationImplementation.forServiceAccessCM1();
            }
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }
    }
}