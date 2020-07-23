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
        private readonly IDetectMenuTextUseCase _takePictureUseCase;

        public AsyncReactiveCommand TakePictureCommand { get; }

        public ReadOnlyReactivePropertySlim<BitmapImage> Picture { get; }

        public TopViewModel(IRegionManager regionManager,
            IDetectMenuTextUseCase takePictureUseCase)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            _takePictureUseCase = takePictureUseCase ?? throw new ArgumentNullException(nameof(takePictureUseCase));

            TakePictureCommand = new AsyncReactiveCommand()
                .WithSubscribe(TakePictureCommandExecuteAsync)
                .AddTo(Disposables);

            Picture = _takePictureUseCase.RestaurantMenu
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

        private async Task TakePictureCommandExecuteAsync()
        {
            await _takePictureUseCase.TakePictureAsync();
        }
    }
}
