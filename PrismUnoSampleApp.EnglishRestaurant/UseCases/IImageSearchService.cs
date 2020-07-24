using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.UseCases
{
    public interface IImageSearchService
    {
        Task<(Exception error, IEnumerable<ImageInfo> result)> SearchImagesAsync(string searchTerm);
    }
}
