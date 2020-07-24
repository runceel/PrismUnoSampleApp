using Prism.Services.Dialogs;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.Infrastructures.ViewModels;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.ViewModels
{
    public class ImageDialogViewModel : ViewModelBase, IDialogAware
    {
        public string Title => "選択した画像";

        public event Action<IDialogResult> RequestClose;

        public ReactivePropertySlim<string> ImageUri { get; } = new ReactivePropertySlim<string>();

        public ReactiveCommand CloseDialogCommand { get; }

        public ImageDialogViewModel()
        {
            CloseDialogCommand = new ReactiveCommand()
                .WithSubscribe(() => RequestClose?.Invoke(new DialogResult(ButtonResult.OK)))
                .AddTo(Disposables);
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var image = parameters.GetValue<ImageInfo>("image");
            ImageUri.Value = image.Uri;
        }
    }
}
