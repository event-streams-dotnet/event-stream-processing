using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event stream processor with result.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    public interface IEventProcessorWithResult<TResult>
    {
        /// <summary>
        /// Process event stream with result.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task with result.</returns>
        Task<TResult> ProcessWithResult(CancellationToken cancellationToken = default);
    }
}
