using Prism.Services.Dialogs;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures.ViewModels;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class ImageListViewModel : ViewModelBase
    {
        private readonly IDetectMenuTextUseCase _detectMenuTextUseCase;
        private readonly IDialogService _dialogService;

        public ReadOnlyReactivePropertySlim<ReadOnlyObservableCollection<ImageInfo>> Images { get; }

        public ReactivePropertySlim<ImageInfo> SelectedImage { get; } = new ReactivePropertySlim<ImageInfo>();

        public ImageListViewModel(IDetectMenuTextUseCase detectMenuTextUseCase, IDialogService dialogService)
        {
            _detectMenuTextUseCase = detectMenuTextUseCase ?? throw new ArgumentNullException(nameof(detectMenuTextUseCase));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            Images = _detectMenuTextUseCase
                .RestaurantMenu
                .CurrentText
                .Select(x => x?.Images)
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
