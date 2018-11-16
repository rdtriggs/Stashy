using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stashy.Api.Infrastructure.Models;

namespace Stashy.Api.Infrastructure.Services
{
    public interface ICoinService
    {
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Coin>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<TDto>> GetAllAsync<TDto>(CancellationToken cancellationToken = default);
        Task<Coin> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<TDto> GetByIdAsync<TDto>(string id, CancellationToken cancellationToken = default);
    }
}
