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
            var connectionManager = host.Services.GetService<ConnectionManager>();
            await connectionManager.StartAsync();

            var myInfoListener = host.Services.GetRequiredService<MyInformationRepository>();
            await myInfoListener.StartAsync();

            var conversationListener = host.Services.GetRequiredService<MyConversationsRepository>();
            await conversationListener.StartAsync();

            var myMessagesListener = host.Services.GetRequiredService<MyMessagesRepository>();
            await myMessagesListener.StartAsync();

            var userChangesListener = host.Services.GetRequiredService<UserChangesRepository>();
            await userChangesListener.StartAsync();

            var mainLoginSignView = host.Services.GetRequiredService<MainLoginSignView>();
            mainLoginSignView.Show();

            base.OnStartup(e);

        }
        protected override void OnExit(ExitEventArgs e)
        {
         
           var connection = ServiceProvider.GetService<ConnectionManager>();
            connection.Close();
        }
        private void ServiceCollections(IServiceCollection service)
        {
            service.AddTransient<IMessageServices,MessagesServices>();
            service.AddTransient<IConversationServices,ConversationServices>();
            service.AddTransient<IProfileServices, ProfileServices>();

            service.AddTransient<MainLoginSignView>();
            service.AddTransient<LoginView>();
            service.AddTransient<SignView>();
            service.AddTransient<MainView>();
            service.AddTransient<ConversationView>();
            service.AddTransient<EditMessageView>();
            service.AddTransient<AddNewConversationView>();
            service.AddTransient<ProfileView>();
            service.AddTransient<LoginLoadingPageView>();
            service.AddTransient<ContactProfileVeiw>();
            service.AddTransient<DefaultMessageView>();
            service.AddTransient<AddNewConverPageView>();

            service.AddSingleton<ConnectionManager>();
            service.AddHostedService<ReconnectManager>();
            service.AddSingleton(s =>
            {
                return new HubConnectionBuilder()
                    .WithUrl("http://localhost:5209/MainHub")
                    .WithAutomaticReconnect()
                    .Build();
            });

            service.AddSingleton<MyInformationRepository>();
            service.AddSingleton<MyMessagesRepository>();
            service.AddSingleton<MyConversationsRepository>();
            service.AddSingleton<UserChangesRepository>();



            service.AddTransient<ProfileViewModel>();
            service.AddSingleton<MainViewModel>();
            service.AddTransient<SignViewModel>();
            service.AddTransient<LoginViewModel>();
            service.AddTransient<MainLoginSignViewModel>(); 
            service.AddSingleton<SharingDataViewModel>();
            service.AddTransient<ContactProfileViewModel>();
            service.AddTransient<EditMessageViewModel>();
            service.AddTransient<ConversationViewModel>();
            service.AddTransient<Func<ConversationViewModel>>(sp => () => sp.GetRequiredService<ConversationViewModel>());
            service.AddTransient<AddNewConversationViewModel>();
        }
    }
    

}
