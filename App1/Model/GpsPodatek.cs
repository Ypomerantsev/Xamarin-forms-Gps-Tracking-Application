using SQLite;
using System;
using Plugin.Geolocator.Abstractions;

namespace App1.Model
{
    public class GpsPodatek
    {
        [PrimaryKey]
        public DateTimeOffset Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Hitrost { get; set; }
        public double Glava { get; set; }
        public double Natancnost { get; set; }
        public double NadmorskaVisina { get; set; }
        public double NatansnostNMV { get; set; } //natancnost nadmorske visine
        public bool PoslanNaStreznik { get; set; }

        /*public GpsPodatek(Position pos)
        {
            TimeStamp = pos.Timestamp;
            Latitude = pos.Latitude;
            Longitude = pos.Longitude;
            Hitrost = pos.Speed;
            PoslanNaStreznik = false;
            Glava = pos.Heading;
            Natancnost = pos.Accuracy;
            NatansnostNMV = pos.AltitudeAccuracy;
            NadmorskaVisina = pos.Altitude;
            
        }*/
        public void SetData(Position pos)
        {
            Timestamp = pos.Timestamp;
            Latitude = pos.Latitude;
            Longitude = pos.Longitude;
            Hitrost = pos.Speed;
            PoslanNaStreznik = false;
            Glava = pos.Heading;
            Natancnost = pos.Accuracy;
            NatansnostNMV = pos.AltitudeAccuracy;
            NadmorskaVisina = pos.Altitude;
        }
    }
}
