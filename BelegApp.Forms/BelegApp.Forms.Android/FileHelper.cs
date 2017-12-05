using System;
using System.IO;

using BelegApp.Forms.Backend;
using BelegApp.Forms.Backend.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BelegApp.Forms.Backend.Droid
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