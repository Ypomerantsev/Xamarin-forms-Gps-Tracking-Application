using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms.Maps;
using System.Globalization;

using System.Diagnostics;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using App1;
//using App1.Model;
using System.Threading.Tasks;
using App1.PodatkovnaBaza;
using App1.PodatkovnaBaza.Modeli;
//using App1.

namespace App1.Sensors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrikazPospeskov : ContentPage
    {

        //public LimitedOCy<Plugin.Geolocator.Abstractions.Position> Kolekcija { get; set; }

        //public OstaliSenzorji ostali;

        static DatabaseFor3DData database;

        public static DatabaseFor3DData Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseFor3DData(DependencyService.Get<IFileHelper>().GetLocalFilePath("Baza3DPodatkov.db3"));
                }
                return database;
            }
        }


        public PrikazPospeskov()
        {
            var limit = 100;
            InitializeComponent();

            //var test = DependencyService.Get<IFileHelper>().GetLocalFilePath("BazaGpsPodatkov.db3");


            DependencyService.Get<ICommunication>().ConnectReference(this);
            LoadBase();




        }
        public void setItemSource(List<TriDimPodatek> list)
        {

            lv.ItemsSource = list;
            
        }
        public async void LoadBase()
        {
            lv.ItemsSource = await Database.GetAllData();
            
        }
    }
}