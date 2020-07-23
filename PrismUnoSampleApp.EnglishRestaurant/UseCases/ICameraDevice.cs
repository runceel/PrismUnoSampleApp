using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public interface ICameraDevice
    {
        Task<byte[]> TakePictureAsync();
    }
}