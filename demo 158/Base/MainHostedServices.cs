using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using demo_158.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace demo_158.Base
{
    internal class MainHostedServices(MainLoginSignView mainLoginSignView) :IHostedService
    {

        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            mainLoginSignView.Show();

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
         return Task.CompletedTask;
        }
    }
}
