using System;
using System.IO;
using BelegApp.Forms.iOS.Utils;
using BelegApp.Forms.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace BelegApp.Forms.iOS.Utils
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}