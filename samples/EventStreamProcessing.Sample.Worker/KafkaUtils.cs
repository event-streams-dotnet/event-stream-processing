using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EventStreamProcessing.Sample.Worker
{
    public static class KafkaUtils
    {
        public static IConsumer<int, string> CreateConsumer(string brokerList,
            List<string> topics, ILogger logger)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = brokerList,
                GroupId = "sample-consumer",
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            var consumer = new ConsumerBuilder<int, string>(config)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => logger.LogInformation($"Error: {e.Reason}"))
                // .SetStatisticsHandler((_, json) => logger.LogInformation($"Statistics: {json}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    logger.LogInformation($"Assigned partitions: [{string.Join(", ", partitions)}]");
                    // possibly manually specify start offsets or override the partition assignment provided by
                    // the consumer group by returning a list of topic/partition/offsets to assign to, e.g.:
                    // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    logger.LogInformation($"Revoking assignment: [{string.Join(", ", partitions)}]");
                })
                .Build();

            consumer.Subscribe(topics);
            return consumer;
        }

        public static IProducer<int, string> CreateProducer(string brokerList,
            string topic, ILogger logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = brokerList
            };

            var producer = new ProducerBuilder<int, string>(config).Build();

            return producer;
        }
    }
}
