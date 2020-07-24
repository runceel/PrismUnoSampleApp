using Microsoft.Extensions.Configuration;
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
            // configs
            containerRegistry.RegisterSingleton<OcrConfiguration>(provider => 
                provider.Resolve<IConfiguration>().GetSection("ocr").Get<OcrConfiguration>());
            containerRegistry.RegisterSingleton<ImageSearchConfiguration>(provider =>
                provider.Resolve<IConfiguration>().GetSection("imageSearch").Get<ImageSearchConfiguration>());

            // domains
            containerRegistry.RegisterSingleton<RestaurantMenu>();

            // services
            containerRegistry.RegisterSingleton<IPictureTextReader, PictureTextReader>();
            containerRegistry.RegisterSingleton<IImageSearchService, ImageSearchService>();

            // usecases
            containerRegistry.Register<IDetectMenuTextUseCase, DetectMenuTextUseCase>();

            // views and viewmodels
            containerRegistry.RegisterForNavigation<TopView, TopViewModel>();
            containerRegistry.RegisterForNavigation<MenuListView, MenuListViewModel>();
            containerRegistry.RegisterForNavigation<ImageListView, ImageListViewModel>();
            containerRegistry.RegisterForNavigation<CommandBarView, TopViewModel>();

            // dialogs and viewmodels
            containerRegistry.RegisterDialog<ImageDialogView, ImageDialogViewModel>();
        }
    }
}
