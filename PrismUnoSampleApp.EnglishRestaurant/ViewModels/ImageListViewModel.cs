using Prism.Services.Dialogs;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures.Http;
using PrismUnoSampleApp.Infrastructures.ViewModels;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class ImageListViewModel : ViewModelBase
    {
        private readonly IDetectMenuTextUseCase _detectMenuTextUseCase;
        private readonly IDialogService _dialogService;
        private readonly IRestClient _client;

        public ReadOnlyReactivePropertySlim<ReadOnlyObservableCollection<ImageInfo>> Images { get; }

        public ReactivePropertySlim<ImageInfo> SelectedImage { get; } = new ReactivePropertySlim<ImageInfo>();

        public ReadOnlyReactivePropertySlim<ImageBrush> BackgroundBrush { get; }

        public ImageListViewModel(IDetectMenuTextUseCase detectMenuTextUseCase,
            IDialogService dialogService,
            IRestClient client)
        {
            _detectMenuTextUseCase = detectMenuTextUseCase ?? throw new ArgumentNullException(nameof(detectMenuTextUseCase));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _client = client ?? throw new ArgumentNullException(nameof(client));

            Images = _detectMenuTextUseCase
                .RestaurantMenu
                .CurrentText
                .Select(x => x?.Images)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(Disposables);

            var random = new Random();
            BackgroundBrush = Images
                .Where(x => x != null)
                .SelectMany(x => x.CollectionChangedAsObservable().ToUnit())
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(_ => Images.Value.ToArray())
                .Where(x => x.Any())
                .Select(x => x.ElementAt(random.Next(x.Length)))
                .SelectMany(async x => await _client.GetAsync(x.Uri))
                .Where(x => x.IsSuccessStatusCode)
                .SelectMany(async x => await x.Content.ReadAsStreamAsync())
                .ObserveOnUIDispatcher()
                .SelectMany(async x =>
                {
                    using var s = x;
                    using var rs = s.AsRandomAccessStream();
                    var image = new BitmapImage();
                    await image.SetSourceAsync(rs);
                    return new ImageBrush { ImageSource = image };
                })
                .OnErrorRetry()
                .Where(x => x != null)
                .ObserveOnUIDispatcher()
                .ToReadOnlyReactivePropertySlim()
                .AddTo(Disposables);

            SelectedImage.Where(x => x != null)
                .Subscribe(x =>
                {
                    _dialogService.ShowDialog(ViewNames.ImageDialogView,
                        new DialogParameters { { "image", x } },
                        _ => { });
                });
        }
    }
}
