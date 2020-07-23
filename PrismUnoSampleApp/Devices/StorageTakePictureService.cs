using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace PrismUnoSampleApp.Devices
{
    public class StorageTakePictureService : ITakePictureService
    {
        public async Task<byte[]> TakePictureAsync()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
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
