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
using App1;


using App1.Droid;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using App1.Sensors;

using Newtonsoft.Json;

[assembly: Dependency(typeof(App1.Droid.Json))]
namespace App1.Droid
{


    public class Json : IJson
    {


        string CreatePathToFile(string filename)
        {
            var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(docsPath, filename);
        }



        public string LoadJson(string fromWhere)
        {
            FileHelper fh = new FileHelper();
            //String location = fh.GetLocalFilePath(fromWhere);
            String location = CreatePathToFile(fromWhere);
            String jssson;
            using (StreamReader file = File.OpenText(location))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    jssson = (String)serializer.Deserialize(file, typeof(String));
                }

            return jssson;//JsonConvert.DeserializeObject<String>(location);
        }

        public void SaveToJson(string what2save)
        {
            JsonSerializer serializer = new JsonSerializer();
            FileHelper fh = new FileHelper();
            using (StreamWriter sw = new StreamWriter(CreatePathToFile("username.json")))
                using(JsonWriter writer = new JsonTextWriter(sw))
{
                    serializer.Serialize(writer, what2save);
                  // {"ExpiryDate":new Date(1230375600000),"Price":0}
}
        }
    }
}