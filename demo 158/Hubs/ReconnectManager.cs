using demo_158.MVVM.Model;
using demo_158.Repository;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace demo_158.Hubs;

public class ReconnectManager (ConnectionManager connectionManager,MyInformationRepository inforepRepository): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (connectionManager.ConnectionState == HubConnectionState.Disconnected )
            {
                await connectionManager.StartAsync();
                if (connectionManager.ConnectionState == HubConnectionState.Connected && inforepRepository.MyUserInfo != null)
                {
                    var user = new UserModelFromUser()
                    {
                        Username = inforepRepository.MyUserInfo.Username,
                    };

                    await connectionManager.SendAsync("ReconnectRequest", user);
                }

            }

            await Task.Delay(TimeSpan.FromSeconds(7),stoppingToken);
        }


    }
}