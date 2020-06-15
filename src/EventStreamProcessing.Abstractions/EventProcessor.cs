using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public abstract class EventProcessor<TSourceEvent, TSinkEvent> : EventProcessorBase, IEventProcessor
    {
        protected readonly IEventConsumer<TSourceEvent> consumer;
        protected readonly IEventProducer<TSinkEvent> producer;

        public EventProcessor(
            IEventConsumer<TSourceEvent> consumer,
            IEventProducer<TSinkEvent> producer,
            params IMessageHandler[] handlers)
            : base(handlers)
        {
            this.consumer = consumer;
            this.producer = producer;
        }

        public abstract Task Process(CancellationToken cancellationToken = default);
    }
}
