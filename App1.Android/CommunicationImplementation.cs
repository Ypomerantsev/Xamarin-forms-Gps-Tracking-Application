using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Content;
using static App1.Droid.Servis;
using System.Threading;


using System.Diagnostics;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Text.RegularExpressions;



using App1.Droid;
using App1.Sensors;

using App1.PodatkovnaBaza;
using App1.PodatkovnaBaza.Modeli;

[assembly: Dependency(typeof(CommunicationImplementation))]
namespace App1.Droid
{

    
    public class CommunicationImplementation : ICommunication
    {
        static Sensors.OstaliSenzorji os;
        static Sensors.PrikazGpsLokacij pgl;
        static Sensors.PrikazPospeskov pp;
        Intent intent;

        public CommunicationImplementation()
        {
            
        }

        public void Connect(Sensors.OstaliSenzorji mpl)
        {

            MainActivity activity = Forms.Context as MainActivity;
            os = mpl;
            intent = new Intent(activity, typeof(Servis));
            intent.SetAction("zazeni"); //2. način preverjanja večih instanc

            activity.StartService(intent);

            /*if (IsmMyServiceRunning())
            {
                Console.WriteLine("ok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\n");
            }*/

            try
            {
                activity.BindService((Intent)intent, (ServiceConnection)activity.mConnection, Bind.AutoCreate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                activity.UnbindService(activity.mConnection);
            }


        }

        public  void NewGpsData(Position e)
        {
            //os.array.AddWithTrigger(e); //old array
            //os.Kolekcija.Add(e);
            if (pgl.Kolekcija!=null){
                pgl.Kolekcija.Add(e);
            }
        }

        public Sensors.OstaliSenzorji getReference()
        {
            return os;
        }

        public void SaveKolekcija()
        {
            DependencyService.Get<ILoadSave<Plugin.Geolocator.Abstractions.Position>>().Save(pgl.Kolekcija,"kolekcija");
        }

        public void Connect(Sensors.PrikazGpsLokacij mpl)
        {

            MainActivity activity = Forms.Context as MainActivity;


            pgl = mpl;
            intent = new Intent(activity, typeof(Servis));
            intent.SetAction("zazeni"); //2. način preverjanja večih instanc

            activity.StartService(intent);

            /*if (IsmMyServiceRunning())
            {
                Console.WriteLine("ok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\nok\n");
            }*/

            try
            {
                activity.BindService((Intent)intent, (ServiceConnection)activity.mConnection, Bind.AutoCreate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                activity.UnbindService(activity.mConnection);
            }

        }

        public void ConnectReference(Sensors.PrikazPospeskov mpl)
        {
            pp = mpl;
        }

        public async void CustomMethod()
        {
            List<GpsPodatek> x = await PrikazGpsLokacij.Database.GetItemsNotSent();
            pgl.setItemSource(x);
        }

        public async void CustomMethod2()
        {
            List<TriDimPodatek> x = await PrikazPospeskov.Database.GetAllData();
            pp.setItemSource(x);
        }

        
        public async static void forServiceAccessCM1()
        {
            List<GpsPodatek> x = await PrikazGpsLokacij.Database.GetItemsNotSent();
            pgl.setItemSource(x);
        }

    }
}