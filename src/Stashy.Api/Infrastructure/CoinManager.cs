using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stashy.Api.Infrastructure.Exceptions;
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
                if (_lastUpdate.AddMinutes(5) < DateTime.Now)
                {
                    _logger.LogDebug("CoinManager is polling");

                    try
                    {
                        await _coinService.UpdateAsync(cancellationToken);
                        _lastUpdate = DateTime.Now;
                    }
                    catch (Exception e)
                    {
                        if (!(e is LoggedException))
                        {
                            _logger.LogError(e, "CoinManager -> RunAsync");
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
