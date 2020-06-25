using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event stream processor with result.
    /// </summary>
    /// <typeparam name="TSourceEvent">Source event type.</typeparam>
    /// <typeparam name="TSinkEvent">Sink event type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    public abstract class EventProcessorWithResult<TSourceEvent, TSinkEvent, TResult> 
        : EventProcessorBase, IEventProcessorWithResult<TResult>
    {
        /// <summary>
        /// Event consumer.
        /// </summary>
        protected readonly IEventConsumer<TSourceEvent> consumer;

        /// <summary>
        /// Event consumer.
        /// </summary>
        protected readonly IEventProducerAsync<TSinkEvent, TResult> producer;

        /// <summary>
        /// Event stream processor constructor.
        /// </summary>
        /// <param name="consumer">Event consumer.</param>
        /// <param name="producer">Event producer.</param>
        /// <param name="handlers">Event handlers.</param>
        public EventProcessorWithResult(
            IEventConsumer<TSourceEvent> consumer,
            IEventProducerAsync<TSinkEvent, TResult> producer,
            params IMessageHandler[] handlers)
            : base(handlers)
        {
            this.consumer = consumer;
            this.producer = producer;
        }

        /// <summary>
        /// Process event stream with result.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task with result.</returns>
        public abstract Task<TResult> ProcessWithResult(CancellationToken cancellationToken = default);
    }
}
