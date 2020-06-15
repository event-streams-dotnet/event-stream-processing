using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public abstract class EventProcessorWithResult<TSourceEvent, TSinkEvent, TResult> 
        : EventProcessorBase, IEventProcessorWithResult<TResult>
    {
        protected readonly IEventConsumer<TSourceEvent> consumer;
        protected readonly IEventProducerAsync<TSinkEvent, TResult> producer;

        public EventProcessorWithResult(
            IEventConsumer<TSourceEvent> consumer,
            IEventProducerAsync<TSinkEvent, TResult> producer,
            params IMessageHandler[] handlers)
            : base(handlers)
        {
            this.consumer = consumer;
            this.producer = producer;
        }

        public abstract Task<TResult> ProcessWithResult(CancellationToken cancellationToken = default);
    }
}
