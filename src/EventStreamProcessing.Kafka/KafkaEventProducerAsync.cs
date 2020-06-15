using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventStreamProcessing.Kafka
{
    public class KafkaEventProducerAsync<TKey, TValue>
        : IEventProducerAsync<Confluent.Kafka.Message<TKey, TValue>, Confluent.Kafka.DeliveryResult<TKey, TValue>>
    {
        private readonly Confluent.Kafka.IProducer<TKey, TValue> producer;
        private readonly string topic;
        private readonly ILogger logger;

        public KafkaEventProducerAsync(Confluent.Kafka.IProducer<TKey, TValue> producer, string topic, ILogger logger = null)
        {
            this.producer = producer;
            this.topic = topic;
            this.logger = logger;
        }

        public async Task<Confluent.Kafka.DeliveryResult<TKey, TValue>> ProduceEventAsync(Confluent.Kafka.Message<TKey, TValue> sinkEvent)
        {
            var result = await producer.ProduceAsync(topic, sinkEvent);
            logger?.LogInformation($"Message produced: {sinkEvent.Key} {sinkEvent.Value}");
            return result;
        }
    }
}
