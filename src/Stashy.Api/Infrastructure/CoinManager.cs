using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stashy.Api.Infrastructure.Services;

namespace Stashy.Api.Infrastructure
{
    public class CoinManager : ICoinManager
    {
        private readonly ICoinService _coinService;
        private readonly ILogger<CoinManager> _logger;
        private DateTime _lastUpdate = DateTime.MinValue;

        public CoinManager(ILogger<CoinManager> logger, ICoinService coinService)
        {
            _logger = logger;
            _coinService = coinService;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await DoWorkAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "{Class} -> {Method} failed, cooling off for 2 minutes", nameof(CoinManager),
                        nameof(RunAsync));

                    await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
                }
            }
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("{Class} is polling", nameof(CoinManager));

            if (_lastUpdate.AddMinutes(5) < DateTime.Now)
            {
                _logger.LogDebug("Updating coins via the coin service");
                _lastUpdate = DateTime.Now;
                await _coinService.UpdateAsync(cancellationToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}
