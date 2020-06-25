using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;

namespace EventStreamProcessing.Kafka
{
    /// <summary>
    /// Kafka event producer.
    /// </summary>
    /// <typeparam name="TKey">Message key type.</typeparam>
    /// <typeparam name="TValue">Message value type.</typeparam>
    public class KafkaEventProducer<TKey, TValue>
        : IEventProducer<Confluent.Kafka.Message<TKey, TValue>>
    {
        private readonly Confluent.Kafka.IProducer<TKey, TValue> producer;
        private readonly string topic;
        private readonly ILogger logger;

        /// <summary>
        /// Kafka event producer constructor.
        /// </summary>
        /// <param name="producer">Kafka producer.</param>
        /// <param name="topic">Kafka topic.</param>
        /// <param name="logger"></param>
        public KafkaEventProducer(Confluent.Kafka.IProducer<TKey, TValue> producer, string topic, ILogger logger = null)
        {
            this.producer = producer;
            this.topic = topic;
            this.logger = logger;
        }

        /// <summary>
        /// Produce Kafka event.
        /// </summary>
        /// <param name="sinkEvent">Kafka sink message.</param>
        public void ProduceEvent(Confluent.Kafka.Message<TKey, TValue> sinkEvent)
        {
            producer.Produce(topic, sinkEvent);
            logger?.LogInformation($"Message produced: {sinkEvent.Key} {sinkEvent.Value}");
        }
    }
}
