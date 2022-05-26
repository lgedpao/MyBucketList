using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyBucketList.Core.Interfaces.Data.Repositories;
using MyBucketList.Mobile.Infrastructure.Data;
using MyBucketList.Mobile.Infrastructure.Data.Repositories.Implementations;
using MyBucketList.Mobile.Infrastructure.Mappings;
using MyBucketList.Mobile.ViewModels;
using MyBucketList.Mobile.Views;
using Prism;
using Prism.Ioc;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace MyBucketList.Mobile
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditBucketlistItemPage, AddEditBucketlistItemPageViewModel>();

            containerRegistry.RegisterServices(s =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new BucketlistItemProfile());
                });

                s.AddSingleton(config.CreateMapper());

                var coreAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("MyBucketList.Core"));
                s.AddMediatR(coreAssemblies.ToArray());
            });

            containerRegistry.RegisterInstance(new DatabaseConnection());
            var dbConnection = Container.Resolve<DatabaseConnection>();
            containerRegistry.RegisterInstance(new Database(dbConnection));
            var database = Container.Resolve<Database>();
            database.InitializeDatabase();

            containerRegistry.RegisterSingleton<IBucketlistItemRepository, BucketlistItemRepository>();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            return base.CreateContainerExtension();
        }
    }
}
