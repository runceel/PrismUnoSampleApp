using PrismUnoSampleApp.EnglishRestaurant.Domains;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class MenuListViewModel : ViewModelBase
    {
        private readonly RestaurantMenu _restaurantMenu;

        public ReadOnlyReactiveCollection<RestaurantMenuItemViewModel> MenuItems { get; }

        public ReactivePropertySlim<RestaurantMenuItemViewModel> SelectedMenuItem { get; } = new ReactivePropertySlim<RestaurantMenuItemViewModel>();

        public MenuListViewModel(RestaurantMenu restaurantMenu)
        {
            _restaurantMenu = restaurantMenu ?? throw new ArgumentNullException(nameof(restaurantMenu));

            MenuItems = restaurantMenu.DetectedTexts
                .ToReadOnlyReactiveCollection(x => new RestaurantMenuItemViewModel(x))
                .AddTo(Disposables);
        }
    }
}
