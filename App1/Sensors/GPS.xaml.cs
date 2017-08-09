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
    public partial class GPS : ContentPage
    {
        int count;
        bool tracking;
        //Position savedPosition;
        public ObservableCollection<Plugin.Geolocator.Abstractions.Position> Positions { get; } = new ObservableCollection<Plugin.Geolocator.Abstractions.Position>();

        public GPS()
        {
            InitializeComponent();
            ListViewPositions.ItemsSource = Positions;
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
                MainPage.os.speedLabel.Text = String.Format("{0:F2}", position.Speed);
                MapsPage.mapRef.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));

            });
        }
    }

}