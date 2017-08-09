using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Globalization;

using Xamarin.Forms;
using System.Diagnostics;
using Plugin.Geolocator;

using App1;


namespace App1
{
    public partial class App : Application 
    {
        public static String ac, co, sp, or, mm; //senzorske spremenljivke

        public static Object parent;

        public static CultureInfo eng;

        public App()
        {
            InitializeComponent();

            /////////////////// lang spec

            System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            var assembly = typeof(App).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            System.Diagnostics.Debug.WriteLine("====================================");

            // This lookup NOT required for Windows platforms - the Culture will be automatically set
            if (Xamarin.Forms.Device.RuntimePlatform == "Android" || Xamarin.Forms.Device.RuntimePlatform == "Ios")
            {
                // determine the correct, supported .NET culture
                eng = new CultureInfo("sl"); // added
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                Resx.AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods

            }
            



            //////////////

            // MainPage = new App1.Page1();
            try
            {
                MainPage = new NavigationPage(new Sensors.MainPage());//new Sensors.MainPage());
            }
            catch (Exception e)
            {
                for (int i = 0; i < 10; i++)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }
        //MainPage = new NavigationPage(new App1.Page1());
        //}

        protected override void OnStart()
        {
            


            
            // Handle when your app starts
            /*
            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer);
            CrossDeviceMotion.Current.Start(MotionSensorType.Gyroscope);
            CrossDeviceMotion.Current.Start(MotionSensorType.Magnetometer);
            CrossDeviceMotion.Current.Start(MotionSensorType.Compass);

            CrossDeviceMotion.Current.SensorValueChanged += (s, a) => {

                switch (a.SensorType)
                {
                    case MotionSensorType.Accelerometer:
                        Debug.WriteLine("A: {0},{1},{2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        ac = "X: " + ((MotionVector)a.Value).X + " Y: "+((MotionVector)a.Value).Y + " :Z " + ((MotionVector)a.Value).Z;

                        break;
                    case MotionSensorType.Gyroscope:
                        Debug.WriteLine("G:  {0},{1},{2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        or = "X: " + ((MotionVector)a.Value).X + " Y: " + ((MotionVector)a.Value).Y + " :Z " + ((MotionVector)a.Value).Z;
                        break;
                    case MotionSensorType.Magnetometer:
                        Debug.WriteLine("M:  {0},{1},{2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        mm = "X: " + ((MotionVector)a.Value).X + " Y: " + ((MotionVector)a.Value).Y + " :Z " + ((MotionVector)a.Value).Z;
                        break;
                    case MotionSensorType.Compass:
                        Debug.WriteLine("H: {0}", a.Value);
                        co = a.Value + "";
                        break;
                }
            };*/
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
