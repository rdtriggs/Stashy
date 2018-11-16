using System.Threading;
using System.Threading.Tasks;

namespace Stashy.Api.Infrastructure
{
    public interface ICoinManager
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
