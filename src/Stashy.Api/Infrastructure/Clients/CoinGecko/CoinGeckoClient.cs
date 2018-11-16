using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using Stashy.Api.Infrastructure.Clients.CoinGecko.Responses;
using Stashy.Api.Infrastructure.Exceptions;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Clients.CoinGecko
{
    public class CoinGeckoClient : Client
    {
        private const string Url = "https://api.coingecko.com";
        private readonly IFlurlClient _flurlClient;
        private readonly ILogger<CoinGeckoClient> _logger;
        private readonly IMapper _mapper;

        public CoinGeckoClient(ILogger<CoinGeckoClient> logger, IFlurlClientFactory flurlClientFactory, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _flurlClient = flurlClientFactory.Get(Url);
        }

        public override async Task<IReadOnlyCollection<Coin>> GetCoinsAsync(
            CancellationToken cancellationToken = default)
        {
            List<Coin> coins = new List<Coin>();
            IReadOnlyCollection<CoinListItem> list = await GetCoinsListAsync(cancellationToken).ConfigureAwait(false);

            if (!list.Any())
            {
                return null;
            }

            const int perPage = 100;
            int totalPages = (int) Math.Ceiling(list.Count / (decimal) perPage);
            List<int> pages = Enumerable.Range(1, totalPages).ToList();
            IEnumerable<Task<IReadOnlyCollection<CoinItem>>> allTasks =
                pages.Select(page => GetCoinsAsync(page, perPage, cancellationToken));

            int batch = 0;
            const int perBatch = 5;
            while (batch < totalPages)
            {
                Task timer = Task.Delay(TimeSpan.FromSeconds(1.2), cancellationToken);
                List<Task<IReadOnlyCollection<CoinItem>>> tasks = allTasks.Skip(batch).Take(perBatch).ToList();
                IEnumerable<Task> tasksAndTimer = tasks.Concat(new[] {timer});

                try
                {
                    _logger.LogDebug("Running batch of requests");

                    // run tasks
                    await Task.WhenAll(tasksAndTimer).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    if (!(e is LoggedException))
                    {
                        _logger.LogError(e, "CoinGeckoClient -> GetCoinsAsync");

                        throw;
                    }
                }

                foreach (Task<IReadOnlyCollection<CoinItem>> task in tasks)
                {
                    if (!task.IsCompletedSuccessfully)
                    {
                        continue;
                    }

                    coins.AddRange(_mapper.Map<IReadOnlyCollection<Coin>>(task.Result));
                }

                batch += perBatch;
            }


            return coins;
        }

        private async Task<IReadOnlyCollection<CoinItem>> GetCoinsAsync(int page, int perPage,
            CancellationToken cancellationToken)
        {
            try
            {
                IReadOnlyCollection<CoinItem> results = await _flurlClient.Request("api/v3/coins")
                    .SetQueryParams(new {per_page = perPage, page})
                    .GetJsonAsync<IReadOnlyCollection<CoinItem>>(cancellationToken);

                return results;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CoinGeckoClient -> GetCoinsAsync (page: {Page}, perPage: {perPage})", page,
                    perPage);

                throw new LoggedException(e);
            }
        }

        private async Task<IReadOnlyCollection<CoinListItem>> GetCoinsListAsync(CancellationToken cancellationToken)
        {
            try
            {
                IReadOnlyCollection<CoinListItem> results = await _flurlClient.Request("api/v3/coins/list")
                    .GetJsonAsync<IReadOnlyCollection<CoinListItem>>(cancellationToken);

                return results;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CoinGeckoClient -> GetCoinsListAsync");

                throw new LoggedException(e);
            }
        }
    }
}
