using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public interface IEventProcessorWithResult<TResult>
    {
        Task<TResult> ProcessWithResult(CancellationToken cancellationToken = default);
    }
}
