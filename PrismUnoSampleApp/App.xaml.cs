using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using PrismUnoSampleApp.Devices;
using PrismUnoSampleApp.EnglishRestaurant;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures;
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
            containerRegistry.RegisterSingleton<ICameraDevice, CameraDevice>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<InfrastructureModule>();
            moduleCatalog.AddModule<EnglishRestaurantModule>();
        }
    }
}
