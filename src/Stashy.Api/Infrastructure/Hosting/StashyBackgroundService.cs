using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stashy.Api.Infrastructure.Exceptions;

namespace Stashy.Api.Infrastructure.Hosting
{
    public class StashyBackgroundService : BackgroundService
    {
        private readonly ILogger<StashyBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StashyBackgroundService(ILogger<StashyBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                try
                {
                    // delay startup
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        ICoinManager coinManager = scope.ServiceProvider.GetService<ICoinManager>();

                        // run coin manager
                        await coinManager.RunAsync(stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    if (!(e is LoggedException))
                    {
                        _logger.LogError(e, "StashyBackgroundService -> ExecuteAsync");
                    }
                }
            }, stoppingToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stashy background service is starting");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stashy background service is stopping");

            return base.StopAsync(cancellationToken);
        }
    }
}
