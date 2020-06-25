using System.Threading;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event consumer.
    /// </summary>
    /// <typeparam name="TSourceEvent">Source event type.</typeparam>
    public interface IEventConsumer<TSourceEvent>
    {
        /// <summary>
        /// Consume event.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Source event.</returns>
        TSourceEvent ConsumeEvent(CancellationToken cancellationToken = default);
    }
}
