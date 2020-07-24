using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public interface IDetectMenuTextUseCase
    {
        RestaurantMenu RestaurantMenu { get; }

        void ClearImages();
        Task<UseCaseResult<int>> LoadMenuImagesAsync(string textId);
        Task <UseCaseResult>ReadPictureTextsAsync();
        Task<UseCaseResult> TakePictureFromCameraAsync();
        Task<UseCaseResult> TakePictureFromStorageAsync();
    }
}
