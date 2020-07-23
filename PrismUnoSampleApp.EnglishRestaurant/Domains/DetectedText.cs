using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

            Text = text;
        }

        public string Text { get; }
    }
}
