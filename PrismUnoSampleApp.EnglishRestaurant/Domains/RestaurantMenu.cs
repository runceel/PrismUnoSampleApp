using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.Domains
{
    public class RestaurantMenu
    {
        public ReactivePropertySlim<byte[]> Picture { get; } = new ReactivePropertySlim<byte[]>();

        private readonly ObservableCollection<DetectedText> _detectedTexts;
        public ReadOnlyObservableCollection<DetectedText> DetectedTexts { get; }

        private readonly ReactivePropertySlim<DetectedText> _currentText = new ReactivePropertySlim<DetectedText>();
        public ReadOnlyReactivePropertySlim<DetectedText> CurrentText { get; }

        public RestaurantMenu()
        {
            _detectedTexts = new ObservableCollection<DetectedText>();
            DetectedTexts = new ReadOnlyObservableCollection<DetectedText>(_detectedTexts);

            CurrentText = _currentText.ToReadOnlyReactivePropertySlim();
        }

        public void ReplaceDetectedTexts(DetectedText[] texts)
        {
            _currentText.Value = null;
            _detectedTexts.Clear();
            foreach (var text in texts ?? Array.Empty<DetectedText>())
            {
                _detectedTexts.Add(text);
            }
        }

        public bool SetCurrentDetectedTextById(string textId) => 
            (_currentText.Value = DetectedTexts.FirstOrDefault(x => x.Id == textId)) != null;
    }
}
