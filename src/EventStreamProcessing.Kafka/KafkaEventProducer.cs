using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;

namespace EventStreamProcessing.Kafka
{
    public class KafkaEventProducer<TKey, TValue>
        : IEventProducer<Confluent.Kafka.Message<TKey, TValue>>
    {
        private readonly Confluent.Kafka.IProducer<TKey, TValue> producer;
        private readonly string topic;
        private readonly ILogger logger;

        public KafkaEventProducer(Confluent.Kafka.IProducer<TKey, TValue> producer, string topic, ILogger logger = null)
        {
            this.producer = producer;
            this.topic = topic;
            this.logger = logger;
        }

        public void ProduceEvent(Confluent.Kafka.Message<TKey, TValue> sinkEvent)
        {
            producer.Produce(topic, sinkEvent);
            logger?.LogInformation($"Message produced: {sinkEvent.Key} {sinkEvent.Value}");
        }
    }
}
