using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Stashy.Api.Infrastructure.Clients;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Services
{
    public class CoinService : ICoinService
    {
        private readonly IClient _client;

        private readonly ConcurrentDictionary<string, Coin> _coins = new ConcurrentDictionary<string, Coin>();
        private readonly IMapper _mapper;

        public CoinService(IClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Coin> coins = await _client.GetCoinsAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (Coin coin in coins)
            {
                _coins.AddOrUpdate(coin.Id, coin, (key, existingVal) => coin);
            }
        }

        public Task<IReadOnlyCollection<Coin>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Coin> result = _coins.Values.OrderBy(coin => coin.Id)
                .ToArray();

            return Task.FromResult(result);
        }

        public async Task<IReadOnlyCollection<TDto>> GetAllAsync<TDto>(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Coin> coins = await GetAllAsync(cancellationToken);
            IReadOnlyCollection<TDto> result = _mapper.Map<IReadOnlyCollection<TDto>>(coins);

            return result;
        }

        public Task<Coin> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            Coin result = _coins.Values.FirstOrDefault(coin => coin.Id == id);

            return Task.FromResult(result);
        }

        public async Task<TDto> GetByIdAsync<TDto>(string id, CancellationToken cancellationToken = default)
        {
            Coin coin = await GetByIdAsync(id, cancellationToken);
            TDto result = _mapper.Map<TDto>(coin);

            return result;
        }
    }
}
