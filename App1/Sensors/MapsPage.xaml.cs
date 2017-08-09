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

namespace App1.Sensors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsPage : ContentPage
    {

        //public static Map mapRef; //pomoje ne bomo rabl ker mamo statične reference na ta inicializiran objekt in lahko kamot dostopamo do njega prek mainPage
        public static Map mapRef; //bomo vseen rabl public referenco ker so private
        public static double _theVariable;
        public static Label tekst;

        public MapsPage()
        {
            
            InitializeComponent();
            mapRef = MyMap;
            //test.Text = CrossGeolocator.Current.DesiredAccuracy.ToString();
            tekst = test;
            //_theVariable = CrossGeolocator.Current.DesiredAccuracy; //to variablo prikazuj ker se spreminja nekak



        }

        
       
    }

    /*public class TipZemljevida
    {
        private String value;
        public delegate void ChangedEventHandler(object sender, EventArgs e);
        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event ChangedEventHandler Changed;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {

            Changed?.Invoke(this, e);
        }

        // Override some of the methods that can change the list;
        // invoke event after each

        public void spremenivrednost(String value)
        {
            this.value = value;
            OnChanged(EventArgs.Empty);
        }

        public void callEvent()
        {
            OnChanged(EventArgs.Empty);
        }

        public void attach(EventTrigger evt)
        {
            // Add "ListChanged" to the Changed event on "List".
            this.Changed += new ChangedEventHandler(evtTrigger);
        }

        // This will be called whenever the list changes.
        public  void evtTrigger(object sender, EventArgs e)
        {
            MapsPage.tekst.Text = value;
        }

        public void Detach()
        {
            // Detach the event and delete the list
            this.Changed -= new ChangedEventHandler(evtTrigger);
        }
    }*/



}