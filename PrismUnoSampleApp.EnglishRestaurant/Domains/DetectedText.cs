using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PrismUnoSampleApp.EnglishRestaurant.Domains
{
    public class DetectedText
    {
        public DetectedText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty", nameof(text));
            }

            Id = Guid.NewGuid().ToString();
            Text = text;

            Images = new ReadOnlyObservableCollection<ImageInfo>(_images);
        }

        public string Id { get; }

        public string Text { get; }

        private readonly ObservableCollection<ImageInfo> _images = new ObservableCollection<ImageInfo>();
        public ReadOnlyObservableCollection<ImageInfo> Images { get; }

        public void ReplaceImages(ImageInfo[] images)
        {
            _images.Clear();
            foreach (var image in images ?? Array.Empty<ImageInfo>())
            {
                _images.Add(image);
            }
        }

        public ImageInfo GetImageInfoById(string imageId) => Images.FirstOrDefault(x => x.Id == imageId);
    }
}
