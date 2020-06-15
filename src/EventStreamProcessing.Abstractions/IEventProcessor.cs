using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public interface IEventProcessor
    {
        Task Process(CancellationToken cancellationToken = default);
    }
}
