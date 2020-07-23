using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public interface ITakePictureService
    {
        Task<byte[]> TakePictureAsync();
    }
}