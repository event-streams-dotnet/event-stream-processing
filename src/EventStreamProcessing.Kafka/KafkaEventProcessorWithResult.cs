using EventStreamProcessing.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Kafka
{
    public class KafkaEventProcessorWithResult<TSourceKey, TSourceValue, TSinkKey, TSinkValue>
        : EventProcessorWithResult<Confluent.Kafka.Message<TSourceKey, TSourceValue>,
            Confluent.Kafka.Message<TSinkKey, TSinkValue>,
            Confluent.Kafka.DeliveryResult<TSinkKey, TSinkValue>>
    {
        public KafkaEventProcessorWithResult(
            IEventConsumer<Confluent.Kafka.Message<TSourceKey, TSourceValue>> consumer,
            IEventProducerAsync<Confluent.Kafka.Message<TSinkKey, TSinkValue>,
                Confluent.Kafka.DeliveryResult<TSinkKey, TSinkValue>> producer, 
            params IMessageHandler[] handlers) 
                : base(consumer, producer, handlers)
        {
        }

        public override async Task<Confluent.Kafka.DeliveryResult<TSinkKey, TSinkValue>> ProcessWithResult(CancellationToken cancellationToken = default)
        {
            // Build chain of handlers
            BuildHandlerChain();

            // Consume event
            var sourceEvent = consumer.ConsumeEvent(cancellationToken);

            // Return null if EOF
            if (sourceEvent == null) return null;

            // Invoke handler chain
            var sourceMessage = new Abstractions.Message<TSourceKey, TSourceValue>(sourceEvent.Key, sourceEvent.Value);
            var sinkMessage = await handlers[0].HandleMessage(sourceMessage) as Abstractions.Message<TSinkKey, TSinkValue>;

            // Produce event
            var sinkEvent = new Confluent.Kafka.Message<TSinkKey, TSinkValue>
            {
                Key = sinkMessage.Key,
                Value = sinkMessage.Value
            };
            return await producer.ProduceEventAsync(sinkEvent);
        }
    }
}
