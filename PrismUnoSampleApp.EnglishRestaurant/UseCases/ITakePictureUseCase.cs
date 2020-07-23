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

        Task ReadPictureTextsAsync();
        Task TakePictureAsync();
    }
}
