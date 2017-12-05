using Windows.Storage;
using System;
using System.IO;

using BelegApp.Forms.Utils;
using BelegApp.Forms.UWP.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BelegApp.Forms.UWP.Utils
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}