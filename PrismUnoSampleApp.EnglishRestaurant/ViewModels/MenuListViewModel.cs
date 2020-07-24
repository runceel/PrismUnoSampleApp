using Prism.Events;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures.Events;
using PrismUnoSampleApp.Infrastructures.ViewModels;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Linq;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class MenuListViewModel : ViewModelBase
    {
        private readonly IDetectMenuTextUseCase _detectMenuTextUseCase;
        private readonly IEventAggregator _eventAggregator;

        public ReadOnlyReactiveCollection<RestaurantMenuItemViewModel> MenuItems { get; }

        public ReactivePropertySlim<RestaurantMenuItemViewModel> SelectedMenuItem { get; } = new ReactivePropertySlim<RestaurantMenuItemViewModel>();

        public MenuListViewModel(IDetectMenuTextUseCase detectMenuTextUseCase, IEventAggregator eventAggregator)
        {
            _detectMenuTextUseCase = detectMenuTextUseCase ?? throw new ArgumentNullException(nameof(detectMenuTextUseCase));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            MenuItems = _detectMenuTextUseCase
                .RestaurantMenu
                .DetectedTexts
                .ToReadOnlyReactiveCollection(x => new RestaurantMenuItemViewModel(x))
                .AddTo(Disposables);

            SelectedMenuItem
                .Subscribe(async x =>
                {
                    if (x == null)
                    {
                        _detectMenuTextUseCase.ClearImages();
                    }
                    else
                    {
                        switch(await _detectMenuTextUseCase.LoadMenuImagesAsync(x.Id))
                        {
                            case { State: UseCaseResultState.Success, Result: var count }:
                                _eventAggregator.GetEvent<UpdateGlobalMessageEvent>().Publish($"{x.Text} の画像を {count} 件取得しました。");
                                break;
                            case { State: UseCaseResultState.Failed, Exception: null }:
                                _eventAggregator.GetEvent<UpdateGlobalMessageEvent>().Publish($"{x.Text} の画像の取得に失敗しました。");
                                break;
                            case { State: UseCaseResultState.Failed, Exception: var ex }:
                                _eventAggregator.GetEvent<UpdateGlobalMessageEvent>().Publish($"{x.Text} の画像の取得に失敗しました。({ex.Message})");
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                })
                .AddTo(Disposables);
        }
    }
}
