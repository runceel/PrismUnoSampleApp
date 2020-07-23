using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public enum ReadPictureTextResult
    {
        Succeed,
        NoPicture,
        Failed,
    }

    public interface IDetectMenuTextUseCase
    {
        RestaurantMenu RestaurantMenu { get; }

        Task <ReadPictureTextResult>ReadPictureTextsAsync();
        Task<bool> TakePictureFromCameraAsync();
        Task<bool> TakePictureFromStorageAsync();
    }
}
