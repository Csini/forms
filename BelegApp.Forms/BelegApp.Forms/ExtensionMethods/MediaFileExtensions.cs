using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;

namespace BelegApp.Forms.ExtensionMethods
{
    static class MediaFileExtensions
    {
        /// <summary>
        /// Liest den Inhalt eines MediaFile-Objektes als byte[].
        /// </summary>
        /// <param name="mediaFile">[in] MediaFile-Objekt.</param>
        /// <returns>Inhalt des MediaFile-Objektes als byte[].</returns>
        public static byte[] GetByteArray(this MediaFile mediaFile)
        {
            byte[] result = null;
            if (mediaFile != null)
            {
                using (BinaryReader binaryReader = new BinaryReader(mediaFile.GetStream()))
                {
                    result = binaryReader.ReadBytes((int)mediaFile.GetStream().Length);
                }
            }

            return result;
        }
    }
}
