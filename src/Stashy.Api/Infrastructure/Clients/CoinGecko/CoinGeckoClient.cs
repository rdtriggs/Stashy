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
            IReadOnlyCollection<CoinListItem> list = await GetCoinsListAsync(cancellationToken).ConfigureAwait(false);

            if (!list.Any())
            {
                return null;
            }

            const int perPage = 100;
            int totalPages = (int) Math.Ceiling(list.Count / (decimal) perPage);
            IReadOnlyCollection<Coin> coins = await ProcessPagesAsync(perPage, totalPages, cancellationToken);

            return coins;
        }

        private async Task<IReadOnlyCollection<Coin>> ProcessPagesAsync(int perPage, int totalPages,
            CancellationToken cancellationToken, int maxDegreeOfParallelism = 10)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
            List<Task<IReadOnlyCollection<CoinItem>>> tasks = new List<Task<IReadOnlyCollection<CoinItem>>>();

            for (int i = 0; i < totalPages; i++)
            {
                int item = i;
                Task<IReadOnlyCollection<CoinItem>> task = Task.Run(async () =>
                {
                    await semaphore.WaitAsync(cancellationToken);

                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(item == 5 ? 1 : 0), cancellationToken);

                        int page = item + 1;
                        IReadOnlyCollection<CoinItem> results = await GetCoinsAsync(page, perPage, cancellationToken);

                        return results;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, cancellationToken);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            IReadOnlyCollection<Coin> coins =
                _mapper.Map<IReadOnlyCollection<Coin>>(tasks.Where(x => x.IsCompletedSuccessfully)
                    .SelectMany(t => t.Result));

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
                _logger.LogDebug("GetCoinsAsync found {Count} coins on page {Page}", results.Count, page);

                return results;
            }
            catch (FlurlHttpException e)
            {
                _logger.LogError(e, "{Class} -> {Method} (page: {Page}, perPage: {PerPage})", nameof(CoinGeckoClient),
                    nameof(GetCoinsAsync), page, perPage);

                throw new LoggedException(e);
            }
        }

        private async Task<IReadOnlyCollection<CoinListItem>> GetCoinsListAsync(CancellationToken cancellationToken)
        {
            try
            {
                IReadOnlyCollection<CoinListItem> results = await _flurlClient.Request("api/v3/coins/list")
                    .GetJsonAsync<IReadOnlyCollection<CoinListItem>>(cancellationToken);
                _logger.LogDebug("GetCoinsListAsync found {Count} coins", results.Count);

                return results;
            }
            catch (FlurlHttpException e)
            {
                _logger.LogError(e, "{Class} -> {Method}", nameof(CoinGeckoClient), nameof(GetCoinsListAsync));

                throw new LoggedException(e);
            }
        }
    }
}
