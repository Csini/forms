using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace BelegApp.Forms.Utils
{
    static class ImageServices
    {
        public static async Task<MediaFile> CaptureImage()
        {
            await CrossMedia.Current.Initialize();

            if ((!CrossMedia.Current.IsCameraAvailable) || (!CrossMedia.Current.IsTakePhotoSupported))
            {
                throw new InvalidOperationException();
            }

            StoreCameraMediaOptions mediaOptions = new StoreCameraMediaOptions()
            {
                Directory = "BelegApp",
                Name = "beleg.jpg"
            };
            MediaFile mediaFile = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

            return mediaFile;
        }

        public static async Task<MediaFile> SelectImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                throw new InvalidOperationException();
            }

            PickMediaOptions mediaOptions = null; //  new PickMediaOptions();
            MediaFile mediaFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            return mediaFile;
        }
    }
}
