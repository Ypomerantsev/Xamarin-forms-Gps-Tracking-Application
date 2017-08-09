using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Util;
using Android.Content;
using Android.Widget;
using Android.OS;

using Android.Hardware;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;


//using Xamarin.Forms; ///////ZAKOMENTIRI KO NE BOS RABU VEC TESTIRAT

using System.Diagnostics;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Threading;

using App1.PodatkovnaBaza;
using App1.PodatkovnaBaza.Modeli;


using App1.Droid.LA;

using System.Net.Http;
using Newtonsoft.Json;

namespace App1.Droid
{
    [Service(Exported = true, Name = "app.droid.Servis")]
    public class Servis : Service
    {
        
        private Binder mBinder;

        //static readonly int highAccuracy = 10;//, lowAccuracy = 500;

        static readonly string TAG = typeof(Servis).FullName;
        //public static double latitude, longtitude, speed, altitude;+
        //static double prevlat, prevlong;
        public static DateTimeOffset timestamp;
        Thread thread;

        Wifi_coarse lowController;
        public App1.Sensors.TipZemljevida tz;


        //Handler handler;
        Handler handlermain;
        bool isStarted = false;

        public static bool EnableSaveAcceleration = true; //v prihodnosti bojo to s streznika krmilili

        GpsPodatek gp;
        TriDimPodatek tdp;

        //Plugin.Geolocator.Abstractions.Position LastPosition; //not needed, just use ma ref

        //////////////////TS//////////
        Database database;
        public Database Database
        {
            get
            {
                var test = ((FileHelper)new FileHelper()).GetLocalFilePath("BazaGpsPodatkov.db3");
                if (database == null)
                {
                    //database = new App1.Database.DB(DependencyService.Get<IFileHelper>().GetLocalFilePath("test.db3"));
                    database = new Database(((FileHelper)new FileHelper()).GetLocalFilePath("BazaGpsPodatkov.db3"));
                }
                return database;
            }
        }

        DatabaseFor3DData database3D;
        public DatabaseFor3DData Database3D
        {
            get
            {
                var test = ((FileHelper)new FileHelper()).GetLocalFilePath("Baza3DPodatkov.db3");
                if (database3D == null)
                {
                    //database = new App1.Database.DB(DependencyService.Get<IFileHelper>().GetLocalFilePath("test.db3"));
                    database3D = new DatabaseFor3DData(((FileHelper)new FileHelper()).GetLocalFilePath("Baza3DPodatkov.db3"));
                }
                return database3D;
            }
        }

        ///////////////////////////////////////////



        public LinearAccelerationImplementation lai;

        public string username;

        public override void OnCreate()
        {
            mBinder = new LocalBinder(this);

            base.OnCreate();

            Log.Debug(TAG, "OnCreate");


            //Toast.MakeText(this, "Pride do Servisa", ToastLength.Long).Show();
            /*if (MainActivity.InstanceCount < 1)
            {
                StartActivity(new Intent(this,typeof(MainActivity)));
            }*/

            //handler = new Handler();
            handlermain = new Handler(Looper.MainLooper);



            //var test = ((FileHelper)new FileHelper()).GetLocalFilePath("BazaGpsPodatkov.db3");


            lowController = new Wifi_coarse(this);


            lowController.Start();
            //StartTrack(); //ta ali pa lowcontroller.start() je lahko naenkrat przgan drugac bo exception unhandled

            tz = new Sensors.TipZemljevida(); // za hendlanje izpisa na mapspageu
            tz.attach(tz);


            tdp = new TriDimPodatek();
            gp = new GpsPodatek();
            gp.resetAccelerationData();
            gp.resetLinearAccelerationData();

            //LastPosition = null;

            try
            {
                //LoadSave<string> ls = new LoadSave<string>();
                //username = ls.LoadJson("username.json");
                Json js = new Json();
                username = js.LoadJson("username.json");
            }catch(Exception e)
            {
                username = Guid.NewGuid().ToString();
            }

            lai = new LinearAccelerationImplementation();
            

            lai.SensorValueChanged += (s, a) =>
            {
                
                //TODO - dodej še 12 polj za podatke linearnega senzorja v gpspodatek DB

                switch (a.SensorType)
                {
                    case SensorType.Accelerometer:
                        gp.CheckAccelerationData(((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z, DateTimeOffset.Now);
                        tdp.Set(TriDimPodatek.Tip.Accelerometer,((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z,DateTimeOffset.Now);
                        break;

                    case SensorType.LinearAcceleration:
                        gp.CheckLinearAccelerationData(((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z, DateTimeOffset.Now);
                        tdp.Set(TriDimPodatek.Tip.LinearAcceleration, ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z, DateTimeOffset.Now);
                        break;
                    default:
                        Console.WriteLine("noben senzor ne ustreza temu");
                        return;
                }

                Database3D.SaveItemAsync(tdp);
            };

            try
            {
                sendToServer();
            }catch(Exception e)
            {

            }

        }


        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals("zazeni"))
            {
                Toast toast;
                if (isStarted)
                {
                    Log.Info(TAG, "OnStartCommand: The service is already running.");
                    toast = Toast.MakeText(this, "ze poganja decko", ToastLength.Long);
                }
                else
                {
                    toast = Toast.MakeText(this, "zaganjam", ToastLength.Long);
                    Log.Info(TAG, "OnStartCommand: The service is starting.");
                    RegisterForegroundService();
                    //handler.PostDelayed(runnable, 100); //COULD USE TO LIMIT SENSOR INPUTS
                    isStarted = true;
                }
                toast.Show();
            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }

        void RegisterForegroundService()
        {
            var notification = new Notification.Builder(this)
                .SetContentTitle("App1")
                .SetContentText("servis v ozadju teče")
                .SetOngoing(true)
                .Build();


            // Enlist this instance of the service as a foreground service
            StartForeground(1241, notification);

        }




        public override IBinder OnBind(Intent intent)
        {
            return mBinder;
        }
        public override void OnDestroy()
        {
            mBinder = null;
        }

        public class LocalBinder : Binder
        {
            Servis s_;   //C# ne vidi izven vgnezdenega razreda
            public LocalBinder(Servis s)
            {
                s_ = s;
            }
            public Servis GetService()
            {
                return s_;
            }
        }









        public async void StartTrack()
        {


            var hasPermission = await Utils.CheckPermissions(Permission.Location);
            if (!hasPermission)
                return;
            
            

            var locator = CrossGeolocator.Current;

            

            tz.spremenivrednost("GPS");

            if (CrossGeolocator.Current.IsListening) //način z removeupdates ne deluje in noče več poslušati na novo, zato pustimo coarse location enabled
            {
                return;
            }

            ////////////////last pos on//////////////////
            ////////////coarse-fine switch///////////////
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
            if (position == null)
            {
                position = new Position();
            }
            gp.SetLocationData(position, DateTimeOffset.Now, GpsPodatek.SwitchType.Vklop);
            await Database.SaveItemAsync(gp);
            MainActivity.Poz = position;

            /////////////////////////////////////////////


            lai.StartAll();

            CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
            CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;

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
            try
            {

                //Device.BeginInvokeOnMainThread(() =>
                //{
                var position = e.Position;


                

                gp.SetLocationData(position,DateTimeOffset.Now,GpsPodatek.SwitchType.Podatek);
                //TODO malenkost bolje, vendar znaš še bolje?

                if (EnableSaveAcceleration)  //TODO mislm da si tuki neki narobe razumu, povpraši!
                {
                    Database.SaveItemAsync(gp);
                }

                MainActivity.Poz = position;
                /*if (EnableSaveAcceleration && (gp.MaxAccX_TS!=null || !lai.IsActive(SensorType.Accelerometer)) && (gp.LinMaxAccX_TS != null || !lai.IsActive(SensorType.LinearAcceleration))) //in pogoj je preventiva ker se praktično ne more ponovit le v primeru da se 2x isto pokliče event
                {
                    Database.SaveItemAsync(gp);
                }else if()// TODO DODEJ TUKI SE ELSE IF POGOJ ČE SO VSAJ GPS PODATKI DA VSAJ TE V BAZO RUKNE
                {
                    
                }*/
                gp.resetLinearAccelerationData();
                gp.resetAccelerationData();




                //TODO tuki pošilji na mainactivity podatke gps tm pa pol na XForms -> IMPLEMENTED FOR CURRENT NEEDS

                ////////////////////////////////////////
                //NOTE:XAMARIN FORMS STVARI NE KLICAT V SERVISU, PA APLIKACIJSKE STVARI RAJŠ V APLIKACIJ SPREMINJI, ANDROID APP KER JE ANDROID SPECIFIC SERVIS
                //IN NE MORE SERVISA POSLUŠAT FORMS...//
                //UpdateMaps(position);
                //DependencyService.Get<ICommunication>().NewGpsData(position); 
                ////////////////////////////////////////

                //});


                if (thread != null)
                {
                    thread.Abort();
                }
                thread = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Thread.Sleep(5000); //11000 default
                //et.callEvent();
                Looper.Prepare();
                    TimeoutThread();
                    handlermain.Post(() =>
                    //Device.BeginInvokeOnMainThread(() =>
                    {
                        gp.SetLocationData(MainActivity.Poz, DateTimeOffset.Now, GpsPodatek.SwitchType.Izklop);
                        Database.SaveItemAsync(gp);
                        MainActivity.Poz = MainActivity.Poz; //just to trigger ui refresh

                        tz.spremenivrednost("Omrežje");
                    });

                });
                thread.Start();

            }catch(Exception ex)
            {
                //Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        public void TimeoutThread()
        {
            

            CrossGeolocator.Current.StopListeningAsync();
            //lowController.Start();
        }

        public void sendToServer()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            var timer = new System.Threading.Timer((e) =>
            {
                MetodaZaObdelavoPodtkov();
            }, null, startTimeSpan, periodTimeSpan);
        }

        public async void MetodaZaObdelavoPodtkov() //TODO izboljsi
        {
            List<GpsPodatek> lpm = await App1.Sensors.PrikazGpsLokacij.Database.GetItemsNotSent();
            DataPacket dp = new DataPacket(lpm, username);

            string output = JsonConvert.SerializeObject(dp);
            Console.WriteLine(output);

            //string output = JsonConvert.SerializeObject(lpm);


            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(output);

                try
                {
                    var response = await client.PostAsync("http://192.168.1.175/OwinApp1/sql", content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    foreach (GpsPodatek gp in lpm)
                    {
                        gp.Poslano();
                        await App1.Sensors.PrikazGpsLokacij.Database.Update(gp);
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message + e.StackTrace);
                }

            }


            
            
        }
    }
}