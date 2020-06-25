using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace EventStreamProcessing.Kafka
{
    /// <summary>
    /// Kafka event consumer.
    /// </summary>
    /// <typeparam name="TKey">Message key type.</typeparam>
    /// <typeparam name="TValue">Message value type.</typeparam>
    public class KafkaEventConsumer<TKey, TValue> : IEventConsumer<Confluent.Kafka.Message<TKey, TValue>>
    {
        private readonly Confluent.Kafka.IConsumer<TKey, TValue> consumer;
        private readonly ILogger logger;

        /// <summary>
        /// Kafka event consumer constructor.
        /// </summary>
        /// <param name="consumer">Kafka consumer.</param>
        /// <param name="logger">Logger.</param>
        public KafkaEventConsumer(Confluent.Kafka.IConsumer<TKey, TValue> consumer, ILogger logger = null)
        {
            this.consumer = consumer;
            this.logger = logger;
        }

        /// <summary>
        /// Consume Kafka event.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Kafka message.</returns>
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
