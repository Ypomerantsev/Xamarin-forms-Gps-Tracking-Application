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

using System.Diagnostics;
using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Text.RegularExpressions;
using System.Threading;

namespace App1
{
    public interface ICommunication
    {
        void NewGpsData(Position e);
        void Connect(Sensors.OstaliSenzorji mpl);
        void Connect(Sensors.PrikazGpsLokacij pgl);
        void ConnectReference(Sensors.PrikazPospeskov mpl);
        void SaveKolekcija();
        void CustomMethod(); //zbrises lahk to ko ne bs vec rabu
        void CustomMethod2(); //zbrises lahk to ko ne bs vec rabu
    }
    
}
