using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures;
using PrismUnoSampleApp.Infrastructures.Events;
using PrismUnoSampleApp.Infrastructures.ViewModels;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class TopViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IDetectMenuTextUseCase _detectMenuTextUseCase;
        private readonly IEventAggregator _eventAggregator;

        public AsyncReactiveCommand TakePictureFromCameraCommand { get; }
        public AsyncReactiveCommand TakePictureFromStorageCommand { get; }

        public ReadOnlyReactivePropertySlim<BitmapImage> Picture { get; }

        public TopViewModel(IRegionManager regionManager,
            IDetectMenuTextUseCase detectMenuTextUseCase,
            IEventAggregator eventAggregator)
        {
            _detectMenuTextUseCase = detectMenuTextUseCase;
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            var sharedCommandState = new ReactivePropertySlim<bool>(true);
            TakePictureFromStorageCommand = new AsyncReactiveCommand(sharedCommandState)
                .WithSubscribe(() => TakePictureCommandExecuteInternalAsync(_detectMenuTextUseCase.TakePictureFromStorageAsync))
                .AddTo(Disposables);

            TakePictureFromCameraCommand = new AsyncReactiveCommand(sharedCommandState)
                .WithSubscribe(() => TakePictureCommandExecuteInternalAsync(_detectMenuTextUseCase.TakePictureFromCameraAsync))
                .AddTo(Disposables);

            Picture = _detectMenuTextUseCase.RestaurantMenu
                .Picture
                .ObserveOnUIDispatcher()
                .SelectMany(x => ByteArrayToBitmapImageAsync(x))
                .ObserveOnUIDispatcher()
                .ToReadOnlyReactivePropertySlim()
                .AddTo(Disposables);
        }

        private static async Task<BitmapImage> ByteArrayToBitmapImageAsync(byte[] picture)
        {
            if (picture == null)
            {
                return null;
            }

            using (var ms = new MemoryStream(picture))
            {
                var image = new BitmapImage();
                await image.SetSourceAsync(ms.AsRandomAccessStream());
                return image;
            }
        }

        private async Task TakePictureCommandExecuteInternalAsync(Func<Task<UseCaseResult>> takePicture)
        {
            if (await takePicture() is { Exception: null, State: UseCaseResultState.Success })
            {
                _eventAggregator.GetEvent<UpdateGlobalMessageEvent>().Publish("");
                switch (await _detectMenuTextUseCase.ReadPictureTextsAsync())
                {
                    case { State: UseCaseResultState.Success }:
                        _regionManager.RequestNavigate(RegionNames.MasterRegion, ViewNames.MenuListView);
                        _regionManager.RequestNavigate(RegionNames.DetailsRegion, ViewNames.ImageListView);
                        _regionManager.RequestNavigate(RegionNames.TopMenuRegion, ViewNames.CommandBarView);
                        break;
                    case { State: UseCaseResultState.Failed, Exception: var ex }:
                        _eventAggregator.GetEvent<UpdateGlobalMessageEvent>().Publish($"写真に書かれている文字を読めませんでした。{ex?.Message}");
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                _eventAggregator.GetEvent<UpdateGlobalMessageEvent>().Publish("写真を選択してください");
            }
        }
    }
}
