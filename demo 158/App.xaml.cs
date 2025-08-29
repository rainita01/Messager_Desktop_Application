
using System.Configuration;
using System.Data;
using System.Windows;
using demo_158.Base;

using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.MVVM.ViewModel;
using demo_158.Services;
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
        protected override async void OnStartup(StartupEventArgs e)
        {
            var applicationBuilder = Host.CreateApplicationBuilder(e.Args);
            applicationBuilder.Configuration.AddJsonFile("appsettings.json");
            applicationBuilder.Services.AddHostedService<MainHostedServices>();
            ServiceCollections(applicationBuilder.Services);

            var hosted = applicationBuilder.Build();
            await hosted.StartAsync();
            base.OnStartup(e);
        }

        private void ServiceCollections(IServiceCollection service)
        {
            service.AddTransient<MessageAndTalkServices>();

            service.AddSingleton<SocketManager>();

            service.AddTransient<MainLoginSignView>();
            service.AddTransient<LoginView>();
            service.AddTransient<SignView>();
            service.AddTransient<MainView>();
            service.AddTransient<MessageAndTalkView>();
            service.AddTransient<ProfileViewModel>();
            service.AddTransient<LoginLoadingPage>();
            service.AddTransient<ContactProfileVeiw>();

            service.AddTransient<DefaultMessageView>();


            
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
