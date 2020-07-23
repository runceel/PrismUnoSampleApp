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
    public class CameraTakePictureService : ITakePictureService
    {
        public async Task<byte[]> TakePictureAsync()
        {
            var camera = new CameraCaptureUI();
            var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file == null)
            {
                return null;
            }

            using (var s = await file.OpenReadAsync())
            using (var ms = new MemoryStream())
            {
                await s.AsStreamForRead().CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
