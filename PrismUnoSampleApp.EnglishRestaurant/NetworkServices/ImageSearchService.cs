using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.NetworkServices
{
    public class ImageSearchService : IImageSearchService
    {
        private readonly ImageSearchConfiguration _imageSearchConfiguration;
        private readonly ImageSearchClient _client;

        public ImageSearchService(ImageSearchConfiguration imageSearchConfiguration)
        {
            _imageSearchConfiguration = imageSearchConfiguration;
            _client = new ImageSearchClient(new ApiKeyServiceClientCredentials(imageSearchConfiguration.SubscriptionKey));
        }

        public async Task<(Exception error, IEnumerable<ImageInfo> result)> SearchImagesAsync(string searchTerm)
        {
            try
            {
                var result = await _client.Images.SearchAsync(searchTerm);
                var images = result.Value.Select(x => new ImageInfo(x.ThumbnailUrl, x.ContentUrl)).ToArray();
                return (null, images);
            }
            catch (Exception ex)
            {
                return (ex, null);
            }
        }
    }
}
