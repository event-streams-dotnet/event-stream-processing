using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event stream processor.
    /// </summary>
    public interface IEventProcessor
    {
        /// <summary>
        /// Process event.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task which will complete when Process finishes.</returns>
        Task Process(CancellationToken cancellationToken = default);
    }
}
