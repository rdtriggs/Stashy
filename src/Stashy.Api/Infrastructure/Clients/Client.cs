using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Clients
{
    public abstract class Client : IClient
    {
        public abstract Task<IReadOnlyCollection<Coin>> GetCoinsAsync(CancellationToken cancellationToken = default);
    }
}
