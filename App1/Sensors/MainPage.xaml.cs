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

namespace App1.Sensors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        //public static GPS gps = new GPS();
        public static OstaliSenzorji os = new OstaliSenzorji();
        public static MapsPage mp = new MapsPage();
        public static PrikazGpsLokacij pgl = new PrikazGpsLokacij();
        public static PrikazPospeskov pp = new PrikazPospeskov();

        public MainPage()
        {
            InitializeComponent();

            //Children.Add(gps);
            Children.Add(pgl);
            Children.Add(pp);
            Children.Add(os);
            Children.Add(mp);


        }
    }
}