using PrismUnoSampleApp.EnglishRestaurant.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class RestaurantMenuItemViewModel
    {
        private readonly DetectedText _text;

        public RestaurantMenuItemViewModel(DetectedText text)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string Id => _text.Id;
        public string Text => _text.Text;
    }
}
