using System.Windows;
using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.MVVM.View;
using demo_158.MVVM.View.Model;
using demo_158.MVVM.ViewModel;
using demo_158.Repository;
using demo_158.Services;
using demo_158.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace demo_158
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider;
        protected override async  void OnStartup(StartupEventArgs e)
        {
            var builder = Host.CreateApplicationBuilder(e.Args);
            builder.Configuration.AddJsonFile("appsettings.json");
            ServiceCollections(builder.Services);

            // Build host
            var host = builder.Build();
            ServiceProvider = host.Services;
            // Start all IHostedServices
            await host.StartAsync();

            // Get ConnectionManager
            var connectionManager = host.Services.GetRequiredService<ConnectionManager>();
            await connectionManager.StartAsync();
            
            var myInfoListener = host.Services.GetRequiredService<MyInformationRepository>();
            await myInfoListener.StartAsync();

            var conversationListener = host.Services.GetRequiredService<MyConversationsRepository>();
            await conversationListener.StartAsync();

            var myMessagesListener = host.Services.GetRequiredService<MyMessagesRepository>();
            await myMessagesListener.StartAsync();

            var mainLoginSignView = host.Services.GetRequiredService<MainLoginSignView>();
            mainLoginSignView.Show();

            base.OnStartup(e);

        }
        protected override void OnExit(ExitEventArgs e)
        {
            
        }
        private void ServiceCollections(IServiceCollection service)
        {
            service.AddTransient<IMessageServices,MessagesServices>();
            service.AddTransient<IConversationServices,ConversationServices>();

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

            service.AddSingleton<MyInformationRepository>();
            service.AddSingleton<MyMessagesRepository>();
            service.AddSingleton<ConnectionStateManager>();
            service.AddSingleton<MyConversationsRepository>();
            service.AddHostedService<ConnectionStateManager>(sp => sp.GetRequiredService<ConnectionStateManager>());


            service.AddTransient<ProfileView>();
            service.AddSingleton<MainViewModel>();
            service.AddTransient<SignViewModel>();
            service.AddTransient<LoginViewModel>();
            service.AddTransient<MainLoginSignViewModel>(); 
            service.AddSingleton<SharingDataViewModel>();
            service.AddTransient<ContactProfileViewModel>();
            service.AddTransient<ProfileModel>();
            service.AddTransient<ConversationModel>();
            service.AddTransient<Func<ConversationModel>>(sp => () => sp.GetRequiredService<ConversationModel>());
        }
    }
    

}
