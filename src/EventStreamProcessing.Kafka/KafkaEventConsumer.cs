using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace EventStreamProcessing.Kafka
{
    public class KafkaEventConsumer<TKey, TValue> : IEventConsumer<Confluent.Kafka.Message<TKey, TValue>>
    {
        private readonly Confluent.Kafka.IConsumer<TKey, TValue> consumer;
        private readonly ILogger logger;

        public KafkaEventConsumer(Confluent.Kafka.IConsumer<TKey, TValue> consumer, ILogger logger = null)
        {
            this.consumer = consumer;
            this.logger = logger;
        }

        public Confluent.Kafka.Message<TKey, TValue> ConsumeEvent(CancellationToken cancellationToken = default)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            if(consumeResult.Message != null)
            {
                logger?.LogInformation($"Message consumed: {consumeResult.Message.Key} {consumeResult.Message.Value}");
            }
            return consumeResult.Message;
        }
    }
}
