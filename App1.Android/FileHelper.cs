using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;
using Xamarin.Forms;
using App1.Droid;


[assembly: Dependency(typeof(FileHelper))]
namespace App1.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}