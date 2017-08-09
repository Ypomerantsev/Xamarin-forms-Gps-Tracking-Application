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

namespace App1.Sensors
{
    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OstaliSenzorji : ContentPage
    {
        public Label speedLabel;
        public static bool coarse_switch = false;
        //public ObservableCollection<string> ListViewItems { get; set; }


        //public ObservableCollection<Plugin.Geolocator.Abstractions.Position> Collection ;

        /*private ObservableCollection<Plugin.Geolocator.Abstractions.Position> _collection;
        public ObservableCollection<Plugin.Geolocator.Abstractions.Position> Collection
        {
            get
            {
                return _collection;
            }
            set
            {
                _collection = value;
            }
        }*/

        //public LimitedOCy<Plugin.Geolocator.Abstractions.Position> Kolekcija { get; set; }

        //public ArrayWithListener<Plugin.Geolocator.Abstractions.Position> array;

        public OstaliSenzorji()
        {
            //this.PropertyChanged+= (sender, e) => { e.PropertyName }
            //array = new ArrayWithListener<Plugin.Geolocator.Abstractions.Position>(this,100); //to spremen pol tko da bo iz zadne verzije pobral ven podatke ne pa na novo nrdil

            //Collection = new ObservableCollection<Plugin.Geolocator.Abstractions.Position>();
            //array.attach(array);
            /*var limit = 100;*/
            //DependencyService.Get<ICommunication>().Connect(this);

            InitializeComponent();

            //_collection = new ObservableCollection<Plugin.Geolocator.Abstractions.Position>();
            //Kolekcija = new LimitedOCy<Plugin.Geolocator.Abstractions.Position>(limit);

            /*try
            {
                Kolekcija = (LimitedOCy<Plugin.Geolocator.Abstractions.Position>)DependencyService.Get<ILoadSave<Plugin.Geolocator.Abstractions.Position>>().Load("kolekcija");
            }catch(Exception e)
            {
                Kolekcija = new LimitedOCy<Plugin.Geolocator.Abstractions.Position>(limit);
            }*/




            //lv.ItemsSource = Kolekcija;

            speedLabel = speed;

            
            //CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer);
            CrossDeviceMotion.Current.Start(MotionSensorType.Gyroscope);
            //CrossDeviceMotion.Current.Start(MotionSensorType.Magnetometer);
            CrossDeviceMotion.Current.Start(MotionSensorType.Compass,(MotionSensorDelay)500);

            
            

            CrossDeviceMotion.Current.SensorValueChanged += (s, a) =>
            {

                switch (a.SensorType)
                {
                    case MotionSensorType.Accelerometer:
                        Debug.WriteLine("A: {0},{1},{2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        App.ac = String.Format("X: {0:F2}\nY: {1:F2}\nZ: {2:F2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        acc.Text = App.ac;

                        break;
                    case MotionSensorType.Gyroscope:
                        Debug.WriteLine("G:  {0},{1},{2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        App.or = String.Format("X: {0:F2}\nY: {1:F2}\nZ: {2:F2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        orient.Text = App.or;
                        break;
                    case MotionSensorType.Compass:
                        Debug.WriteLine("H: {0}", a.Value);
                        //String temp = (a.Value + "").Replace("Value = ","");

                        //String temp = Regex.Match(a.Value+"", @"[-+]?[0-9]*,[0-9]2").Value; //da dobimo samo številko
                        //double temp = Double.Parse(a.Value.Value+"");
                        //WHATEVER, ne rabmo tega ma .value.value že željeno vrednost

                        App.co = String.Format("{0:F2}", a.Value.Value);
                        compas.Text = App.co;
                        break;
                }

                Debug.WriteLine(System.Globalization.CultureInfo.CurrentCulture);
                
            };
        }

        //public double Ax { get; set; }

        private void slo_Clicked(object sender, EventArgs e)
        {
            //var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            App.eng = new CultureInfo("sl");
            Resx.AppResources.Culture = App.eng; // set the RESX for resource localization
            DependencyService.Get<ILocalize>().SetLocale(App.eng); // set the Thread for locale-aware methods
            Debug.WriteLine(System.Globalization.CultureInfo.CurrentCulture);
            InitializeComponent();
        }


        private void en_Clicked(object sender, EventArgs e)
        {
            App.eng = new CultureInfo("en-US");
            Resx.AppResources.Culture = App.eng;
            DependencyService.Get<ILocalize>().SetLocale(App.eng); // set the Thread for locale-aware methods
            Debug.WriteLine(System.Globalization.CultureInfo.CurrentCulture);
            InitializeComponent();

        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            coarse_switch = !coarse_switch;
            Debug.WriteLine("Hello world {0}",coarse_switch);
        }

        private void inputTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DependencyService.Get<IJson>().SaveToJson(inputTest.Text);
            }
            catch(Exception exa)
            {
                Debug.WriteLine(exa.Message);
            }
        }



        /*public void OnChange()
        {
            new ListViewHandler(lv, array);
            ListViewHandler.RefreshList();
        }*/
    }



    //metoda iz mojega prejšnjega arraya
    /*public class ListViewHandler
    {
        static ArrayWithListener<Plugin.Geolocator.Abstractions.Position> target;
        static ListView lv;

        public ListViewHandler(ListView lve,ArrayWithListener<Plugin.Geolocator.Abstractions.Position> tgt)
        {
            target = tgt;
            lv = lve;
        }

        public static String[] vrednosti(ArrayWithListener<Plugin.Geolocator.Abstractions.Position> target,int length)
        {
            String[] novo = new string[length];
            int i = 0;
            foreach(Plugin.Geolocator.Abstractions.Position s in target)
            {
                novo[i] = "Lattitude:" + s.Latitude + "\nLongtitude" + s.Longitude; //TODO se ostale podatke sam za testne namene je to dost
                i++;
            }

            return novo;
        }

        public static void RefreshList()
        {
            lv.ItemsSource = vrednosti(target,target.Count);
        }
    }*/

    public class SensorData : INotifyPropertyChanged
    {
        public double AccelerometerX { get; set; }
        public double AccelerometerY { get; set; }
        public double AccelerometerZ { get; set; }
        public void Update()
        {
            AccelerometerX++;
            AccelerometerY++;
            AccelerometerZ++;

            PropertyChanged(this, new PropertyChangedEventArgs("AccelerometerX"));

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    //[XmlInclude(typeof(LimitedOCy<Plugin.Geolocator.Abstractions.Position>))]
    //[XmlInclude(typeof(LimitedOCy<>))]
    public class LimitedOCy<T> : ObservableCollection<T>
    {
        int limit;

        public LimitedOCy() : base()
        {
            this.limit = 100;
        }

        public LimitedOCy(int limit) : base()
        {
            this.limit = limit;
        }
        public void Add(T e)
        {
            base.Add(e);
            if (Count > limit)
            {
                RemoveAt(0);
            }
        }

        


    }
}