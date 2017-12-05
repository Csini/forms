using Windows.Storage;
using System;
using System.IO;

using BelegApp.Forms.Backend;
using BelegApp.Forms.Backend.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BelegApp.Forms.Backend.UWP
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}