using EventStreamProcessing.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Kafka
{
    /// <summary>
    /// Kafka event processor.
    /// </summary>
    /// <typeparam name="TSourceKey">Source message key type.</typeparam>
    /// <typeparam name="TSourceValue">Source message value type.</typeparam>
    /// <typeparam name="TSinkKey">Sink message key type.</typeparam>
    /// <typeparam name="TSinkValue">Sink message value type.</typeparam>
    public class KafkaEventProcessor<TSourceKey, TSourceValue, TSinkKey, TSinkValue>
        : EventProcessor<Confluent.Kafka.Message<TSourceKey, TSourceValue>,
            Confluent.Kafka.Message<TSinkKey, TSinkValue>>
    {
        /// <summary>
        /// Kafka event processor constructor.
        /// </summary>
        /// <param name="consumer">Event consumer.</param>
        /// <param name="producer">Event producer.</param>
        /// <param name="handlers">Event handlers.</param>
        public KafkaEventProcessor(
            IEventConsumer<Confluent.Kafka.Message<TSourceKey, TSourceValue>> consumer,
            IEventProducer<Confluent.Kafka.Message<TSinkKey, TSinkValue>> producer, 
            params IMessageHandler[] handlers) 
                : base(consumer, producer, handlers)
        {
        }

        /// <summary>
        /// Process event.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task which will complete when Process finishes.</returns>
        public override async Task Process(CancellationToken cancellationToken = default)
        {
            // Build chain of handlers
            BuildHandlerChain();

            // Consume event
            var sourceEvent = consumer.ConsumeEvent(cancellationToken);

            // Return if EOF
            if (sourceEvent == null) return;

            // Invoke handler chain
            var sourceMessage = new Message<TSourceKey, TSourceValue>(sourceEvent.Key, sourceEvent.Value);
            var sinkMessage = await handlers[0].HandleMessage(sourceMessage) as Message<TSinkKey, TSinkValue>;

            // Return if message filtered out
            if (sinkMessage == null) return;

            // Produce event
            var sinkEvent = new Confluent.Kafka.Message<TSinkKey, TSinkValue>
            {
                Key = sinkMessage.Key,
                Value = sinkMessage.Value
            };
            producer.ProduceEvent(sinkEvent);
        }
    }
}
