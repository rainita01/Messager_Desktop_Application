
using System.Configuration;
using System.Data;
using System.Windows;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.Midleware;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.MVVM.ViewModel;
using demo_158.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSocketSharp;

namespace demo_158
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider ServiceProvider;
        protected override  void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            InitialCatalog();
        }

        public  void InitialCatalog()
        {
            var services = new ServiceCollection();
            ServiceCollections(services);
            ServiceProvider = services.BuildServiceProvider();


            var connectionManager = ServiceProvider.GetRequiredService<ConnectionManager>();
             connectionManager.StartAsync();

            var mainLoginSignView = ServiceProvider.GetService<MainLoginSignView>();

            mainLoginSignView.Show();
           
        }

        private void ServiceCollections(IServiceCollection service)
        {
            service.AddTransient<MessageAndTalkServices>();

            service.AddTransient<MainLoginSignView>();
            service.AddTransient<LoginView>();
            service.AddTransient<SignView>();
            service.AddTransient<MainView>();
            service.AddTransient<MessageAndTalkView>();
            service.AddTransient<ProfileViewModel>();
            service.AddTransient<LoginLoadingPage>();
            service.AddTransient<ContactProfileVeiw>();

            service.AddTransient<DefaultMessageView>();

            service.AddSingleton<ConnectionManager>();
            service.AddSingleton(s =>
            {
                return new HubConnectionBuilder()
                    .WithUrl("http://localhost:5209/MainHub")
                    .WithAutomaticReconnect()
                    .Build();
            });
            service.AddSingleton<MessageReceiveController>();
            
            service.AddTransient<ProfileView>();
            service.AddTransient<MainViewModel>();
            service.AddTransient<SignViewModel>();
            service.AddTransient<LoginViewModel>();
            service.AddTransient<MainLoginSignViewModel>();
            service.AddTransient<MessageAndTalkViewModel>();
            service.AddSingleton<SharingDataViewModel>();
            service.AddTransient<ContactProfileViewModel>();
            service.AddTransient<ProfileModel>();

            
        }
    }

}
