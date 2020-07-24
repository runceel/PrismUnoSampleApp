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
        private readonly IImageSearchService _imageSearchService;

        public DetectMenuTextUseCase(RestaurantMenu restaurantMenu,
            IPictureTextReader pictureTextReader,
            [Dependency("cameraTakePictureService")]
            ITakePictureService cameraTakePictureService,
            [Dependency("storageTakePictureService")]
            ITakePictureService storageTakePictureService,
            IImageSearchService imageSearchService)
        {
            RestaurantMenu = restaurantMenu ?? throw new ArgumentNullException(nameof(restaurantMenu));
            _pictureTextReader = pictureTextReader ?? throw new ArgumentNullException(nameof(pictureTextReader));
            _cameraTakePictureService = cameraTakePictureService ?? throw new ArgumentNullException(nameof(cameraTakePictureService));
            _storageTakePictureService = storageTakePictureService ?? throw new ArgumentNullException(nameof(storageTakePictureService));
            _imageSearchService = imageSearchService ?? throw new ArgumentNullException(nameof(imageSearchService));
        }

        public RestaurantMenu RestaurantMenu { get; }

        public Task<UseCaseResult> TakePictureFromCameraAsync() => TakePictureAsync(_cameraTakePictureService);

        public Task<UseCaseResult> TakePictureFromStorageAsync() => TakePictureAsync(_storageTakePictureService);

        private async Task<UseCaseResult> TakePictureAsync(ITakePictureService takePictureService)
        {
            try
            {
                RestaurantMenu.Picture.Value = await takePictureService.TakePictureAsync();
                return RestaurantMenu.Picture.Value != null ? UseCaseResult.Success() : UseCaseResult.Failed();
            }
            catch(Exception ex)
            {
                return UseCaseResult.Error(ex);
            }
        }

        public async Task<UseCaseResult> ReadPictureTextsAsync()
        {
            try
            {
                if (RestaurantMenu.Picture.Value == null)
                {
                    return UseCaseResult.Failed();
                }

                var (ok, results) = await _pictureTextReader.ReadTextsAsync(RestaurantMenu.Picture.Value);
                if (ok)
                {
                    RestaurantMenu.ReplaceDetectedTexts(results);
                    return UseCaseResult.Success();
                }
                else
                {
                    RestaurantMenu.ReplaceDetectedTexts(null);
                    return UseCaseResult.Failed();
                }
            }
            catch (Exception ex)
            {
                return UseCaseResult.Error(ex);
            }
        }

        public async Task<UseCaseResult<int>> LoadMenuImagesAsync(string textId)
        {
            ClearImages();
            if(!RestaurantMenu.SetCurrentDetectedTextById(textId))
            {
                return UseCaseResult.Failed(0);
            }

            static UseCaseResult<int> successCase(DetectedText text, IEnumerable<ImageInfo> r)
            {
                text.ReplaceImages(r?.ToArray());
                return UseCaseResult.Success(r?.Count() ?? 0);
            }

            var current = RestaurantMenu.CurrentText.Value;
            return (await _imageSearchService.SearchImagesAsync(current.Text)) switch
            {
                (null, var results) => successCase(current, results),
                (Exception ex, _) => UseCaseResult.Error<int>(ex),
            };
        }

        public void ClearImages()
        {
            foreach (var d in RestaurantMenu.DetectedTexts)
            {
                d.ReplaceImages(null);
            }
        }
    }
}
