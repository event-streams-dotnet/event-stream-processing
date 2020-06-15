using EventStreamProcessing.Abstractions;
using EventStreamProcessing.Kafka;
using EventStreamProcessing.Sample.Worker.Handlers;
using EventStreamProcessing.Sample.Worker.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EventStreamProcessing.Sample.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Add options
                    var consumerOptions = hostContext.Configuration
                        .GetSection(nameof(ConsumerOptions))
                        .Get<ConsumerOptions>();
                    services.AddSingleton(consumerOptions);
                    var producerOptions = hostContext.Configuration
                        .GetSection(nameof(ProducerOptions))
                        .Get<ProducerOptions>();
                    services.AddSingleton(producerOptions);

                    // Add logger
                    services.AddSingleton<ILogger>(sp =>
                        sp.GetRequiredService<ILoggerFactory>().CreateLogger<KafkaWorker>());

                    // Add language store
                    var languageStore = new ConcurrentDictionary<int, string>
                    {
                        [1] = "Ciao",
                        [2] = "Bonjour",
                        [3] = "Hola",
                        [4] = "Hello",
                    };
                    services.AddSingleton<IDictionary<int, string>>(languageStore);

                    // Add event processor
                    services.AddSingleton<IEventProcessor>(sp =>
                    {
                        // Create logger, consumer, producers
                        var logger = sp.GetRequiredService<ILogger>();
                        var kafkaConsumer = KafkaUtils.CreateConsumer(
                            consumerOptions.Brokers, consumerOptions.TopicsList,
                            sp.GetRequiredService<ILogger>());
                        var producerOptions = sp.GetRequiredService<ProducerOptions>();
                        var kafkaErrorProducer = KafkaUtils.CreateProducer(
                            producerOptions.Brokers, producerOptions.ValidationTopic,
                            sp.GetRequiredService<ILogger>());
                        var kafkaFinalProducer = KafkaUtils.CreateProducer(
                            producerOptions.Brokers, producerOptions.FinalTopic,
                            sp.GetRequiredService<ILogger>());

                        // Create handlers
                        var handlers = new List<MessageHandler>
                        {
                            new ValidationHandler(
                                sp.GetRequiredService<IDictionary<int, string>>(),
                                new KafkaEventProducer<int, string>(kafkaErrorProducer, producerOptions.ValidationTopic, logger),
                                logger),
                            new EnrichmentHandler(
                                sp.GetRequiredService<IDictionary<int, string>>(), logger),
                            new FilterHandler(
                                sp.GetRequiredService<IDictionary<int, string>>(),
                                m => !m.Value.Contains("Hello"), logger) // Filter out English greetings
                        };

                        // Create event processor
                        return new KafkaEventProcessor<int, string, int, string>(
                            new KafkaEventConsumer<int, string>(kafkaConsumer, logger),
                            new KafkaEventProducer<int, string>(kafkaFinalProducer, producerOptions.FinalTopic, logger),
                            handlers.ToArray());
                    });

                    // Add worker
                    services.AddHostedService<KafkaWorker>();
                });
            return builder;
        }
    }
}
