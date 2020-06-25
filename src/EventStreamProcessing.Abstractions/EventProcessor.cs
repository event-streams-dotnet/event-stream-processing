using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event stream processor.
    /// </summary>
    /// <typeparam name="TSourceEvent">Source event type.</typeparam>
    /// <typeparam name="TSinkEvent">Sink event type.</typeparam>
    public abstract class EventProcessor<TSourceEvent, TSinkEvent> : EventProcessorBase, IEventProcessor
    {
        /// <summary>
        /// Event consumer.
        /// </summary>
        protected readonly IEventConsumer<TSourceEvent> consumer;

        /// <summary>
        /// Event producer.
        /// </summary>
        protected readonly IEventProducer<TSinkEvent> producer;

        /// <summary>
        /// Event stream processor constructor.
        /// </summary>
        /// <param name="consumer">Event consumer.</param>
        /// <param name="producer">Event producer.</param>
        /// <param name="handlers">Event handlers.</param>
        public EventProcessor(
            IEventConsumer<TSourceEvent> consumer,
            IEventProducer<TSinkEvent> producer,
            params IMessageHandler[] handlers)
            : base(handlers)
        {
            this.consumer = consumer;
            this.producer = producer;
        }

        /// <summary>
        /// Process event stream.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task which will complete when Process finishes.</returns>
        public abstract Task Process(CancellationToken cancellationToken = default);
    }
}
