using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;

namespace PrismUnoSampleApp.Devices
{
    public class CameraDevice : ICameraDevice
    {
        public async Task<byte[]> TakePictureAsync()
        {
            var mediaCapture = new MediaCapture();
            await mediaCapture.InitializeAsync();
            using (var ms = new MemoryStream())
            {
                await mediaCapture.CapturePhotoToStreamAsync(
                    ImageEncodingProperties.CreatePng(),
                    ms.AsRandomAccessStream());
                return ms.ToArray();
            }
        }
    }
}
