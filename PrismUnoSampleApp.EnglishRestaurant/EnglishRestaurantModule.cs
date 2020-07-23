using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.EnglishRestaurant.NetworkServices;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using PrismUnoSampleApp.EnglishRestaurant.ViewModels;
using PrismUnoSampleApp.EnglishRestaurant.Views;
using PrismUnoSampleApp.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant
{
    public class EnglishRestaurantModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.DetailsRegion, ViewNames.TopView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<RestaurantMenu>();
            containerRegistry.RegisterSingleton<IPictureTextReader, PictureTextReader>();
            containerRegistry.Register<IDetectMenuTextUseCase, DetectMenuTextUseCase>();
            containerRegistry.RegisterForNavigation<TopView, TopViewModel>();
        }
    }
}
