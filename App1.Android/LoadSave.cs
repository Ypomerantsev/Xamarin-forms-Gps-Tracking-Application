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

[assembly: Dependency(typeof(App1.Droid.LoadSave<Plugin.Geolocator.Abstractions.Position>))]
namespace App1.Droid
{


    public class LoadSave<T> : ILoadSave<T>
    {

        /*private List<T> Load()
        {

            string file = "filepath";
            List<T> listofa = new List<T>();
            XmlSerializer formatter = new XmlSerializer(T.GetType());
            FileStream aFile = new FileStream(file, FileMode.Open);
            byte[] buffer = new byte[aFile.Length];
            aFile.Read(buffer, 0, (int)aFile.Length);
            MemoryStream stream = new MemoryStream(buffer);
            return (List<T>)formatter.Deserialize(stream);
        }*/


        /*private void Save(List<T> listofa)
        {
            string path = "filepath";
            FileStream outFile = File.Create(path);
            XmlSerializer formatter = new XmlSerializer(T.GetType());
            formatter.Serialize(outFile, listofa);
        }*/


        public void Save(ObservableCollection<T> oc, string whereTo)
        {

            //MainActivity activity = Forms.Context as MainActivity;
            string path = CreatePathToFile(whereTo);
            FileStream outFile = File.Create(path);
            XmlSerializer formatter = new XmlSerializer(typeof(LimitedOCy<T>));
            formatter.Serialize(outFile, oc);
            outFile.Dispose();
        }

        public ObservableCollection<T> Load(string fromWhere)
        {
            string file = CreatePathToFile(fromWhere);
            List<T> listofa = new List<T>();
            XmlSerializer formatter = new XmlSerializer(typeof(LimitedOCy<T>));
            FileStream aFile = new FileStream(file, FileMode.Open);
            byte[] buffer = new byte[aFile.Length];
            aFile.Read(buffer, 0, (int)aFile.Length);
            MemoryStream stream = new MemoryStream(buffer);
            aFile.Dispose();
            return (ObservableCollection<T>)formatter.Deserialize(stream);
        }

        string CreatePathToFile(string filename)
        {
            var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(docsPath, filename);
        }



        public string LoadJson(string fromWhere)
        {
            FileHelper fh = new FileHelper();
            String location = fh.GetLocalFilePath(fromWhere);
            return JsonConvert.DeserializeObject<String>(location);
        }

        public void SaveToJson(string what2save)
        {
            JsonSerializer serializer = new JsonSerializer();
            FileHelper fh = new FileHelper();
            using (StreamWriter sw = new StreamWriter(fh.GetLocalFilePath("username.json")))
                using(JsonWriter writer = new JsonTextWriter(sw))
{
                    serializer.Serialize(writer, what2save);
                  // {"ExpiryDate":new Date(1230375600000),"Price":0}
}
        }
    }
}