using Attendance.WPF.Services;
using Attendance.WPF.Stores;
using Attendance.WPF.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Attendance.WPF
{
    public partial class App : Application
    {
        private readonly IServiceProvider _servicesProvider;

        public IConfigurationRoot Configuration { get; set; }

        public App()
        {
            /*
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            */

            IServiceCollection services = new ServiceCollection();


            //Stores
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<MessageStore>();

            services.AddSingleton<UserStore>();
            services.AddSingleton<ActivityStore>();
            services.AddSingleton<CurrentUser>();


            //Navigation
            services.AddSingleton<INavigationService>(CreateHomeNavigationService);
            services.AddSingleton<CloseModalNavigationService>();


            //ViewModels
            services.AddTransient<HomeViewModel>(CreateHomeViewModel);
            services.AddSingleton<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddTransient<UserMenuViewModel>(CreateUserMenuViewModel);

            services.AddTransient<UserSelectActivityViewModel>(); //CreateUserSelectActivityViewModel
            services.AddTransient<UserDailyOverviewViewModel>();
            services.AddTransient<UsersViewModel>();

            services.AddSingleton<MainViewModel>();

            //MainWindow
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _servicesProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            INavigationService initialNavigationService = _servicesProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();


            MainWindow = _servicesProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
        {
            return new HomeViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                CreateUserMenuNavigationService(serviceProvider),
                serviceProvider.GetRequiredService<CurrentUser>()
                );
        }

        private UserSelectActivityViewModel CreateUserSelectActivityViewModel(IServiceProvider serviceProvider)
        {
            return UserSelectActivityViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<ActivityStore>(),
                serviceProvider.GetRequiredService<CurrentUser>(),
                CreateHomeNavigationService(serviceProvider)
                );
        }

        private INavigationService CreateUsersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UsersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UsersViewModel>());
        }

        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<HomeViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>());
        }

        private INavigationService CreateUserMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UserMenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserMenuViewModel>()
                );//() => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateHomeNavigationService(serviceProvider),
                serviceProvider.GetRequiredService<CurrentUser>()
                );
        }

        private UserMenuViewModel CreateUserMenuViewModel(IServiceProvider serviceProvider)
        {
            return new UserMenuViewModel(
                serviceProvider.GetRequiredService<UserSelectActivityViewModel>(),
                serviceProvider.GetRequiredService<UserDailyOverviewViewModel>(),
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<ActivityStore>()
                );
        }

        
    }
}
