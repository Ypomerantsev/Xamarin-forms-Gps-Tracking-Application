using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;
using Plugin.Geolocator;
using static App1.Droid.Servis;
using System.Threading;
using Xamarin.Forms;
using App1.Sensors;

using App1.PodatkovnaBaza;
using App1.PodatkovnaBaza.Modeli;

using Xamarin.Forms.Maps;


namespace App1.Droid
{
    [Activity(Label = "App1", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity: global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity //, View.IOnClickListener
    {
        Bundle bundle;

        public static double latitude, longtitude, speed, altitude;
        public static DateTimeOffset timestamp;

        public bool isStarted = false, isConnected = false;
        public Servis mService;
        public ServiceConnection mConnection;

        //Intent intent;


        static readonly string SERVICE_STARTED_KEY = "servis";
        //GPS_Service_Connection serviceConnection;//na novo dodan

        private static Plugin.Geolocator.Abstractions.Position _poz;
        public static Plugin.Geolocator.Abstractions.Position Poz {
            get
            {
                return _poz;
            }

            set
            {
                _poz = value;
                UpdateMaps(_poz);
                DependencyService.Get<ICommunication>().NewGpsData(_poz);


                //SQL Logic
                //GpsPodatek gp = new GpsPodatek();
                //gp.SetLocationData(value);
                //PrikazGpsLokacij.Database.SaveItemAsync(gp);
                DependencyService.Get<ICommunication>().CustomMethod();
                //DependencyService.Get<ICommunication>().CustomMethod2(); //NOČEM VSAK SPREMEMBO IZPISVT ACC PODATKE KER BI BIL PREVELIK NAVAL

            }

        }

        /*private static GpsPodatek _gpd;
        public static GpsPodatek Gpd
        {
            get
            {
                return _gpd;
            }
            set
            {
                _gpd = value;
                DependencyService.Get<ICommunication>().CustomMethod();
            }
        }*/



        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            this.bundle = bundle;
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);



            /*           
                        if (serviceConnection == null)
                        {
                            serviceConnection = new GPS_Service_Connection(this);
                        }
                        DoBindService();
                        latitude = GPS_Service_Android.latitude;
                        longtitude = GPS_Service_Android.longtitude;
                        speed = GPS_Service_Android.speed;
                        altitude = GPS_Service_Android.altitude;
                        timestamp = GPS_Service_Android.timestamp;
                        */

            

            mConnection = new ServiceConnection(this);

            if (bundle != null)
            {
                isStarted = bundle.GetBoolean(SERVICE_STARTED_KEY, false);
            }


            
            


            ///////////////////////////////////////////////////
            /*int num = ServiceInstancesRunning();

            while (num>1)
            {
                System.Diagnostics.Debug.Write("odvecen servis je bil prizgan");
                StopService(new Intent(this, typeof(Servis)));
                num--;
            }*/
            /////////////////////////////////////////////////////////


            /*intent = new Intent(this, typeof(Servis));
            intent.SetAction("zazeni"); //2. način preverjanja večih instanc

            StartService(intent);

            */

            /*if (IsmMyServiceRunning())
            {
                Console.WriteLine("ok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\n");
            }*/

            /*try
            {
                BindService((Intent)intent, (ServiceConnection)mConnection, Bind.AutoCreate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                UnbindService(mConnection);
            }*/



            var a = new App();
            //DoBindService();
            LoadApplication(a);

            
            ///IZPIS ACC PODARTKOV VSAK N SEKUNDEN INTERVAL
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(10);

            var timer = new System.Threading.Timer((e) =>
            {
                RunOnUiThread(() =>
                {
                    DependencyService.Get<ICommunication>().CustomMethod2();
                });
            }, null, startTimeSpan, periodTimeSpan);

            ////////////////////



            /*SetContentView(App1.Droid.Resource.Layout.main);
            Button sensbtn = (Button)FindViewById(Resource.Id.sensbtn);
            Button crossbtn = (Button)FindViewById(Resource.Id.crossbtn);
            sensbtn.SetOnClickListener(this);
            crossbtn.SetOnClickListener(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            */

            //global::Xamarin.Forms.Forms.Init(this, bundle);
            //LoadApplication(new App());

            //Intent intent = new Intent(this, typeof(Activity1));
            //StartActivity(intent);



            //NA NOVO DODANO
        }

        protected override void OnStart()
        {
            base.OnStart();

            

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBoolean(SERVICE_STARTED_KEY, isStarted);
            base.OnSaveInstanceState(outState);
            
        }

        /*public void DoBindService()
        {
            Intent serviceToStart = new Intent(this, typeof(GPS_Service_Android));
            BindService(serviceToStart, serviceConnection, Bind.AutoCreate);
        }

        public void DoUnBindService()
        {
            UnbindService(serviceConnection);
        }
         */
        private bool IsmMyServiceRunning()
        {
            ActivityManager am = (ActivityManager)GetSystemService(ActivityService);
            try
            {
                foreach (ActivityManager.RunningServiceInfo service in am.GetRunningServices(int.MaxValue))
                {
                    Console.WriteLine(service.Service.ClassName);
                    if ("app1.droid.Servis".Equals(service.Service.ClassName))
                    {
                        Console.WriteLine("found it\n\n\n");
                        return true;
                    }
                }
                return false;
            }catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return false;
            }
        }

        private int ServiceInstancesRunning()
        {
            ActivityManager am = (ActivityManager)GetSystemService(ActivityService);
            int count = 0;
            try
            {
                foreach (ActivityManager.RunningServiceInfo service in am.GetRunningServices(int.MaxValue))
                {
                    Console.WriteLine(service.Service.ClassName);
                    if ("app1.droid.Servis".Equals(service.Service.ClassName))
                    {
                        Console.WriteLine("found it\n\n\n");
                        count++;
                    }
                }
                return count;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return 0;
            }
        }

        protected override void OnDestroy()
        {
            CrossGeolocator.Current.StopListeningAsync();
            DependencyService.Get<ICommunication>().SaveKolekcija();
            //DoUnBindService();
            base.OnDestroy();
        }

        static void UpdateMaps(Plugin.Geolocator.Abstractions.Position position)
        {
            Sensors.MapsPage.mapRef.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
        }


        /*
        private class LocalBCR : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                throw new NotImplementedException();
            }
        }*/

    }

    /*public class MAGP : GpsPodatek
    {
        public void displayData()
        {
            DependencyService.Get<ICommunication>().CustomMethod();
        }
    }*/
}

