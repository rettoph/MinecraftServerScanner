using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinecraftServerScanner.Server.Controllers;
using MinecraftServerScanner.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MinecraftServerScanner.Server.HostedServices
{
    internal class RescanHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private IServiceProvider _services;
        private Timer _timer;

        private struct ServerCountdown
        {
            public MinecraftServer Server { get; set; }
            public CountdownEvent Countdown { get; set; }
        }


        public RescanHostedService(IServiceProvider services, ILogger<RescanHostedService> logger)
        {
            _logger = logger;
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rescan Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Rescan Service is working.");

            using (var scope = _services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<MinecraftContext>();
                

                // Load 100 servers that havent been scanned in at least 5 mintues
                var servers = db.MinecraftServers
                    .OrderBy(s => s.Scanned)
                    .Take(10);

                // Create a new countdown to wait until the servers are done scanning
                var countdown = new CountdownEvent(servers.Count());

                // queu each server for scanning
                foreach (MinecraftServer server in servers)
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(this.Scan), 
                        new ServerCountdown() { Server = server, Countdown = countdown });

                // Wait for the countdown
                countdown.Wait();

                // update the database changes
                db.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rescan Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void Scan(Object data)
        {
            _logger.LogInformation("Scanning server...");

            ServerCountdown serverContext = (ServerCountdown)data;

            // Scan the server
            serverContext.Server.Scan();

            // Signal the countdown
            serverContext.Countdown.Signal();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
