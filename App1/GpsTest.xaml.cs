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

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GpsTest : TabbedPage
    {
        int count;
        bool tracking;
        //Position savedPosition;
        public ObservableCollection<Plugin.Geolocator.Abstractions.Position> Positions { get; } = new ObservableCollection<Plugin.Geolocator.Abstractions.Position>();

        public GpsTest()
        {
            InitializeComponent();
            ListViewPositions.ItemsSource = Positions;


            CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer);
            CrossDeviceMotion.Current.Start(MotionSensorType.Gyroscope);
            CrossDeviceMotion.Current.Start(MotionSensorType.Magnetometer);
            CrossDeviceMotion.Current.Start(MotionSensorType.Compass);

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
                    /*case MotionSensorType.Magnetometer:
                        Debug.WriteLine("M:  {0},{1},{2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        App.mm = String.Format("X: {0:F2}\nY: {1:F2}\nZ: {2:F2}", ((MotionVector)a.Value).X, ((MotionVector)a.Value).Y, ((MotionVector)a.Value).Z);
                        speed.Text = App.mm;
                        break;*/
                    case MotionSensorType.Compass:
                        Debug.WriteLine("H: {0}", a.Value);
                        //String temp = (a.Value + "").Replace("Value = ","");

                        //String temp = Regex.Match(a.Value+"", @"[-+]?[0-9]*,[0-9]2").Value; //da dobimo samo številko
                        //double temp = Double.Parse(a.Value.Value+"");
                        //WHATEVER, ne rabmo tega ma .value.value že željeno vrednost
                        
                        App.co = String.Format("Vrednost: {0:F2}", a.Value.Value);
                        compas.Text = App.co;
                        break;
                }
            };

            
           

        }

        private async void ButtonTrack_Clicked(object sender, EventArgs e)
        {
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                if (tracking) //ko pržgeš je tracking še na false, zato pri true odstran listener
                {
                    CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;
                }
                else
                {
                    CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
                    CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
                }

                if (CrossGeolocator.Current.IsListening)
                {
                    await CrossGeolocator.Current.StopListeningAsync();
                    labelGPSTrack.Text = "Stopped tracking";
                    ButtonTrack.Text = "Start Tracking";
                    tracking = false;
                    count = 0;
                }
                else
                {
                    Positions.Clear();
                    if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(TrackTimeout.Value), TrackDistance.Value,
                        TrackIncludeHeading.IsToggled, new ListenerSettings
                        {
                            ActivityType = (ActivityType)ActivityTypePicker.SelectedIndex,
                            AllowBackgroundUpdates = AllowBackgroundUpdates.IsToggled,
                            DeferLocationUpdates = DeferUpdates.IsToggled,
                            DeferralDistanceMeters = DeferalDistance.Value,
                            DeferralTime = TimeSpan.FromSeconds(DeferalTIme.Value),
                            ListenForSignificantChanges = ListenForSig.IsToggled,
                            PauseLocationUpdatesAutomatically = PauseLocation.IsToggled
                        }))
                    {
                        labelGPSTrack.Text = "Started tracking";
                        ButtonTrack.Text = "Stop Tracking";
                        tracking = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
                Debug.WriteLine(ex.Message);

            }
        }
        void CrossGeolocator_Current_PositionError(object sender, PositionErrorEventArgs e)
        {

            labelGPSTrack.Text = "Location error: " + e.Error.ToString();
        }

        void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                var position = e.Position;
                Positions.Add(position);
                count++;
                LabelCount.Text = $"{count} updates";
                labelGPSTrack.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
                speed.Text = String.Format("{0:F2}",position.Speed);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude,position.Longitude), Distance.FromMiles(1)));

            });
        }

        

    }
}