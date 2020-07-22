using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using PrismUnoSampleApp.EnglishRestaurant;
using PrismUnoSampleApp.Views;
using Windows.UI.Xaml;

namespace PrismUnoSampleApp
{
    sealed partial class App : PrismApplication
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override UIElement CreateShell() => Container.Resolve<Shell>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<EnglishRestaurantModule>();
        }
    }
}
