using System;

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
        }

        public string Id { get; }

        public string Text { get; }
    }
}
