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
using System.Collections.ObjectModel;

namespace App1
{
    public interface ILoadSave<T>
    {
        void Save(ObservableCollection<T> oc,String whereTo);
        ObservableCollection<T> Load(String fromWhere);
        void SaveToJson(String what2save);
        String LoadJson(String fromWhere);
    }
    
}
