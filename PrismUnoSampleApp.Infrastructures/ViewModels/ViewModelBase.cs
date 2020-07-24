using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System.Reactive.Disposables;

namespace PrismUnoSampleApp.Infrastructures.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();
        public void Destroy() => Disposables.Dispose();

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
