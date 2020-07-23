using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.Domains
{
    public class RestaurantMenu
    {
        public ReactivePropertySlim<byte[]> Picture { get; } = new ReactivePropertySlim<byte[]>();

        private readonly ObservableCollection<DetectedText> _detectedTexts;
        public ReadOnlyObservableCollection<DetectedText> DetectedTexts { get; }

        public RestaurantMenu()
        {
            _detectedTexts = new ObservableCollection<DetectedText>();
            DetectedTexts = new ReadOnlyObservableCollection<DetectedText>(_detectedTexts);
        }

        public void ReplaceDetectedTexts(params DetectedText[] texts)
        {
            _detectedTexts.Clear();
            foreach (var text in texts)
            {
                _detectedTexts.Add(text);
            }
        }
    }
}
