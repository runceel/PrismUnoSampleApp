using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public class DetectMenuTextUseCase : IDetectMenuTextUseCase
    {
        private readonly IPictureTextReader _pictureTextReader;
        private readonly ITakePictureService _cameraTakePictureService;
        private readonly ITakePictureService _storageTakePictureService;

        public DetectMenuTextUseCase(RestaurantMenu restaurantMenu,
            IPictureTextReader pictureTextReader,
            [Dependency("cameraTakePictureService")]
            ITakePictureService cameraTakePictureService,
            [Dependency("storageTakePictureService")]
            ITakePictureService storageTakePictureService)
        {
            RestaurantMenu = restaurantMenu ?? throw new ArgumentNullException(nameof(restaurantMenu));
            _pictureTextReader = pictureTextReader ?? throw new ArgumentNullException(nameof(pictureTextReader));
            _cameraTakePictureService = cameraTakePictureService ?? throw new ArgumentNullException(nameof(cameraTakePictureService));
            _storageTakePictureService = storageTakePictureService ?? throw new ArgumentNullException(nameof(storageTakePictureService));
        }

        public RestaurantMenu RestaurantMenu { get; }

        public Task<bool> TakePictureFromCameraAsync() => TakePictureAsync(_cameraTakePictureService);

        public Task<bool> TakePictureFromStorageAsync() => TakePictureAsync(_storageTakePictureService);

        private async Task<bool> TakePictureAsync(ITakePictureService takePictureService)
        {
            RestaurantMenu.Picture.Value = await takePictureService.TakePictureAsync();
            return RestaurantMenu.Picture.Value != null;
        }

        public async Task<ReadPictureTextResult> ReadPictureTextsAsync()
        {
            if (RestaurantMenu.Picture.Value == null)
            {
                return ReadPictureTextResult.NoPicture;
            }

            var (ok, results) = await _pictureTextReader.ReadTextsAsync(RestaurantMenu.Picture.Value);
            if (ok)
            {
                RestaurantMenu.ReplaceDetectedTexts(results);
                return ReadPictureTextResult.Succeed;
            }
            else
            {
                RestaurantMenu.ReplaceDetectedTexts();
                return ReadPictureTextResult.Failed;
            }
        }

        public async Task GetMenuImagesAsync(DetectedText text)
        {

        }
    }
}
