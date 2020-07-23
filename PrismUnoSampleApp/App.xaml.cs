using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using PrismUnoSampleApp.Devices;
using PrismUnoSampleApp.EnglishRestaurant;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.Infrastructures;
using PrismUnoSampleApp.Views;
using System.Net.Http;
using Windows.ApplicationModel;
using Windows.Storage;
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
            containerRegistry.RegisterSingleton<IConfiguration>(() => new ConfigurationBuilder()
                .SetBasePath(Package.Current.InstalledLocation.Path)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.development.json", true)
                .Build());
            containerRegistry.RegisterInstance(new HttpClient());
            containerRegistry.RegisterSingleton<ITakePictureService, CameraTakePictureService>("cameraTakePictureService");
            containerRegistry.RegisterSingleton<ITakePictureService, StorageTakePictureService>("storageTakePictureService");
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<InfrastructureModule>();
            moduleCatalog.AddModule<EnglishRestaurantModule>();
        }
    }
}
