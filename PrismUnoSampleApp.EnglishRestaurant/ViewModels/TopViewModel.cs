using Prism.Mvvm;
using Prism.Regions;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures;
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

        public ReactivePropertySlim<string> InformationMessage { get; } = new ReactivePropertySlim<string>();

        public AsyncReactiveCommand TakePictureFromCameraCommand { get; }
        public AsyncReactiveCommand TakePictureFromStorageCommand { get; }

        public ReadOnlyReactivePropertySlim<BitmapImage> Picture { get; }

        public TopViewModel(IRegionManager regionManager,
            IDetectMenuTextUseCase detectMenuTextUseCase)
        {
            _detectMenuTextUseCase = detectMenuTextUseCase;
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            TakePictureFromStorageCommand = new AsyncReactiveCommand()
                .WithSubscribe(() => TakePictureCommandExecuteInternalAsync(_detectMenuTextUseCase.TakePictureFromStorageAsync))
                .AddTo(Disposables);

            TakePictureFromCameraCommand = new AsyncReactiveCommand()
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

        private async Task TakePictureCommandExecuteInternalAsync(Func<Task<bool>> takePicture)
        {
            if (await takePicture())
            {
                InformationMessage.Value = "";
                switch(await _detectMenuTextUseCase.ReadPictureTextsAsync())
                {
                    case ReadPictureTextResult.Succeed:
                        _regionManager.RequestNavigate(RegionNames.MasterRegion, ViewNames.MenuListView);
                        break;
                    case ReadPictureTextResult.Failed:
                    case ReadPictureTextResult.NoPicture:
                        InformationMessage.Value = "写真の読み込みに失敗しました。";
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                InformationMessage.Value = "写真を選択してください";
            }
        }
    }
}
