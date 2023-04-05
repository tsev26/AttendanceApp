﻿using Attendance.WPF.Services;
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
            services.AddSingleton<GroupStore>();
            services.AddSingleton<SelectedUserStore>(CreateSelectedUserStore);


            //Navigation
            services.AddSingleton<INavigationService>(CreateHomeNavigationService);
            services.AddSingleton<CloseModalNavigationService>();
            services.AddSingleton<CompositeNavigationService>();
            


            //ViewModels
            services.AddTransient<HomeViewModel>(CreateHomeViewModel);
            services.AddSingleton<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddTransient<UserMenuViewModel>(CreateUserMenuViewModel);

            services.AddTransient<UserSelectActivityViewModel>(CreateUserSelectActivityViewModel);
            services.AddTransient<UserDailyOverviewViewModel>(CreateUserDailyOverviewViewModel);
            services.AddTransient<UsersViewModel>(CreateUsersViewModel);
            services.AddTransient<UserKeysViewModel>(CreateUserKeysViewModel);
            services.AddTransient<UserKeyUpsertViewModel>(CreateUserKeyUpsertViewModel);
            services.AddTransient<UserProfileViewModel>(CreateUserProfileViewModel);
            services.AddTransient<GroupsViewModel>(CreateGroupsViewModel);
            services.AddTransient<GroupUpsertViewModel>(CreateGroupUpsertViewModel);
            services.AddTransient<ActivitiesViewModel>(CreateActivitiesViewModel);
            services.AddTransient<ActivityUpsertViewModel>(CreateActivityUpsertViewModel);
            services.AddTransient<UserHistoryViewModel>(CreateUserHistoryViewModel);
            services.AddTransient<UserUpsertViewModel>(CreateUserUpsertViewModel);
            services.AddTransient<UserSelectActivitySpecialViewModel>(CreateUserSelectActivitySpecialViewModel);
            services.AddTransient<UserHasCurrentlyPlanViewModel>(CreateUserHasCurrentlyPlanViewModel);
            services.AddTransient<UserFixAttendanceRecordViewModel>(CreateUserFixAttendanceRecordViewModel);

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

        private SelectedUserStore CreateSelectedUserStore(IServiceProvider serviceProvider)
        {
            return new SelectedUserStore(
                serviceProvider.GetRequiredService<UserStore>()
                );
        }
        private HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
        {
            return new HomeViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                CreateUserMenuNavigationService(serviceProvider),
                serviceProvider.GetRequiredService<CurrentUser>()
                );
        }

        private UserKeysViewModel CreateUserKeysViewModel(IServiceProvider serviceProvider)
        {
            return new UserKeysViewModel(
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<SelectedUserStore>(),
                CreateUserKeyUpsertNavigationService(serviceProvider)
                );
        }

        private UserSelectActivityViewModel CreateUserSelectActivityViewModel(IServiceProvider serviceProvider)
        {
            return new UserSelectActivityViewModel(
                serviceProvider.GetRequiredService<ActivityStore>(),
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<SelectedUserStore>(),
                CreateHomeNavigationService(serviceProvider),
                CreateUserSelectActivitySpecialNavigationService(serviceProvider)
                );
        }

        private UserKeyUpsertViewModel CreateUserKeyUpsertViewModel(IServiceProvider serviceProvider)
        {
            return new UserKeyUpsertViewModel(
                serviceProvider.GetRequiredService<SelectedUserStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>()
                );
        }

        private INavigationService CreateUsersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UsersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UsersViewModel>());
        }

        private INavigationService CreateUserKeyUpsertNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<UserKeyUpsertViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserKeyUpsertViewModel>());
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
                () => serviceProvider.GetRequiredService<UserMenuViewModel>());
        }

        private INavigationService CreateUserKeyNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UserKeysViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserKeysViewModel>());
        }

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateHomeNavigationService(serviceProvider),
                CreateUserKeyNavigationService(serviceProvider),
                CreateUserMenuNavigationService(serviceProvider),
                CreateUserProfileNavigationService(serviceProvider),
                CreateGroupNavigationService(serviceProvider),
                CreateActivitiesNavigationService(serviceProvider),
                CreateUsersNavigationService(serviceProvider),
                CreateUserHistoryService(serviceProvider),
                serviceProvider.GetRequiredService<CurrentUser>()
                );
        }

        private INavigationService CreateGroupNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<GroupsViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<GroupsViewModel>());
        }

        private INavigationService CreateUserProfileNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UserProfileViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserProfileViewModel>());
        }

        private UserMenuViewModel CreateUserMenuViewModel(IServiceProvider serviceProvider)
        {
            return new UserMenuViewModel(
                serviceProvider.GetRequiredService<UserSelectActivityViewModel>(),
                serviceProvider.GetRequiredService<UserDailyOverviewViewModel>(),
                serviceProvider.GetRequiredService<CurrentUser>(),
                CreateUserHasCurrentlyPlanNavigationService(serviceProvider)
                );
        }

        private UserProfileViewModel CreateUserProfileViewModel(IServiceProvider serviceProvider)
        {
            return new UserProfileViewModel(
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<UserStore>()
                );
        }

        private GroupsViewModel CreateGroupsViewModel(IServiceProvider serviceProvider)
        {
            return new GroupsViewModel(
                serviceProvider.GetRequiredService<GroupStore>(),
                serviceProvider.GetRequiredService<UserStore>(),
                CreateGroupUpsertNavigationService(serviceProvider)

                );
        }

        private INavigationService CreateGroupUpsertNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<GroupUpsertViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<GroupUpsertViewModel>());
        }

        private GroupUpsertViewModel CreateGroupUpsertViewModel(IServiceProvider serviceProvider)
        {
            return new GroupUpsertViewModel(
                serviceProvider.GetRequiredService<GroupStore>(),
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>()
                );
        }

        private ActivitiesViewModel CreateActivitiesViewModel(IServiceProvider serviceProvider)
        {
            return new ActivitiesViewModel(
                serviceProvider.GetRequiredService<ActivityStore>(),
                CreateActivityUpsertNavigationService(serviceProvider)
                );
        }

        private ActivityUpsertViewModel CreateActivityUpsertViewModel(IServiceProvider serviceProvider)
        {
            return new ActivityUpsertViewModel(
                serviceProvider.GetRequiredService<ActivityStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>()
                );
        }

        private INavigationService CreateActivityUpsertNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<ActivityUpsertViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<ActivityUpsertViewModel>());
        }

        private INavigationService CreateActivitiesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<ActivitiesViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<ActivitiesViewModel>());
        }

        private UsersViewModel CreateUsersViewModel(IServiceProvider serviceProvider)
        {
            return new UsersViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<GroupStore>(),
                serviceProvider.GetRequiredService<SelectedUserStore>(),
                CreateUserUpsertNavigationService(serviceProvider),
                CreateUserKeyUpsertNavigationService(serviceProvider)
                );
        }

        private INavigationService CreateUserHistoryService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UserHistoryViewModel>(
                 serviceProvider.GetRequiredService<NavigationStore>(),
                 serviceProvider.GetRequiredService<MessageStore>(),
                 () => serviceProvider.GetRequiredService<UserHistoryViewModel>());
        }

        private UserHistoryViewModel CreateUserHistoryViewModel(IServiceProvider serviceProvider)
        {
            return new UserHistoryViewModel(
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<UserDailyOverviewViewModel>()
                );
        }

        private INavigationService CreateUserUpsertNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<UserUpsertViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserUpsertViewModel>());
        }

        private UserUpsertViewModel CreateUserUpsertViewModel(IServiceProvider serviceProvider)
        {
            return new UserUpsertViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<GroupStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>()                
                );
        }

        private INavigationService CreateUserSelectActivitySpecialNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<UserSelectActivitySpecialViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserSelectActivitySpecialViewModel>());
        }

        private UserSelectActivitySpecialViewModel CreateUserSelectActivitySpecialViewModel(IServiceProvider serviceProvider)
        {
            return new UserSelectActivitySpecialViewModel(
                    serviceProvider.GetRequiredService<CurrentUser>(),
                    serviceProvider.GetRequiredService<SelectedUserStore>(),
                    serviceProvider.GetRequiredService<ActivityStore>(),
                    CreateHomeNavigationService(serviceProvider),
                    serviceProvider.GetRequiredService<CloseModalNavigationService>()
                );
        }

        private UserHasCurrentlyPlanViewModel CreateUserHasCurrentlyPlanViewModel(IServiceProvider serviceProvider)
        {
            return new UserHasCurrentlyPlanViewModel(
                    serviceProvider.GetRequiredService<CurrentUser>(),
                    serviceProvider.GetRequiredService<ActivityStore>(),
                    CreateHomeNavigationService(serviceProvider),
                    serviceProvider.GetRequiredService<CloseModalNavigationService>()
                );
        }

        private INavigationService CreateUserHasCurrentlyPlanNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<UserHasCurrentlyPlanViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserHasCurrentlyPlanViewModel>());
        }

        private UserDailyOverviewViewModel CreateUserDailyOverviewViewModel(IServiceProvider serviceProvider)
        {
            return new UserDailyOverviewViewModel(
                serviceProvider.GetRequiredService<CurrentUser>(),
                serviceProvider.GetRequiredService<SelectedUserStore>(),
                CreateFixAttendanceNavigationService(serviceProvider)
                );
        }

        private INavigationService CreateFixAttendanceNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<UserFixAttendanceRecordViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<UserFixAttendanceRecordViewModel>());
        }

        private UserFixAttendanceRecordViewModel CreateUserFixAttendanceRecordViewModel(IServiceProvider serviceProvider)
        {
            return new UserFixAttendanceRecordViewModel(
                serviceProvider.GetRequiredService<SelectedUserStore>(),
                serviceProvider.GetRequiredService<ActivityStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>()
                );
        }
    }
}
