using System;
using System.IO;

using BelegApp.Forms.Utils;
using BelegApp.Forms.Android.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BelegApp.Forms.Android.Utils
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