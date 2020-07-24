using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.Domains
{
    public class ImageInfo
    {
        public string Id { get; }
        public string ThumbnailUri { get; }
        public string Uri { get; }

        public ImageInfo(string thumbnailUri, string uri)
        {
            Id = Guid.NewGuid().ToString();
            ThumbnailUri = thumbnailUri;
            Uri = uri;
        }
    }
}
