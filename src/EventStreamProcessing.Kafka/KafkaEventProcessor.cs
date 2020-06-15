using EventStreamProcessing.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Kafka
{
    public class KafkaEventProcessor<TSourceKey, TSourceValue, TSinkKey, TSinkValue>
        : EventProcessor<Confluent.Kafka.Message<TSourceKey, TSourceValue>,
            Confluent.Kafka.Message<TSinkKey, TSinkValue>>
    {
        public KafkaEventProcessor(
            IEventConsumer<Confluent.Kafka.Message<TSourceKey, TSourceValue>> consumer,
            IEventProducer<Confluent.Kafka.Message<TSinkKey, TSinkValue>> producer, 
            params IMessageHandler[] handlers) 
                : base(consumer, producer, handlers)
        {
        }

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
