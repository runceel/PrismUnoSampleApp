using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public class DetectMenuTextUseCase : IDetectMenuTextUseCase
    {
        private readonly ICameraDevice _cameraDevice;
        private readonly IPictureTextReader _pictureTextReader;

        public DetectMenuTextUseCase(RestaurantMenu restaurantMenu,
            ICameraDevice cameraDevice,
            IPictureTextReader pictureTextReader)
        {
            RestaurantMenu = restaurantMenu ?? throw new ArgumentNullException(nameof(restaurantMenu));
            _cameraDevice = cameraDevice ?? throw new ArgumentNullException(nameof(cameraDevice));
            _pictureTextReader = pictureTextReader ?? throw new ArgumentNullException(nameof(pictureTextReader));
        }

        public RestaurantMenu RestaurantMenu { get; }

        public async Task TakePictureAsync() => 
            RestaurantMenu.Picture.Value = await _cameraDevice.TakePictureAsync();

        public async Task ReadPictureTextsAsync()
        {
            if (RestaurantMenu.Picture.Value == null)
            {
                return;
            }

            RestaurantMenu.ReplaceDetectedTexts(
                await _pictureTextReader.ReadTextsAsync(RestaurantMenu.Picture.Value));
        }
    }
}
