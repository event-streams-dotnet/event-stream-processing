using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStreamProcessing.Sample.Worker.Handlers
{
    public class ValidationHandler : MessageHandler
    {
        private readonly IDictionary<int, string> languageStore;
        private readonly IEventProducer<Confluent.Kafka.Message<int, string>> validationErrorProducer;
        private readonly ILogger logger;

        public ValidationHandler(IDictionary<int, string> languageStore,
            IEventProducer<Confluent.Kafka.Message<int, string>> validationErrorProducer,
            ILogger logger)
        {
            this.languageStore = languageStore;
            this.validationErrorProducer = validationErrorProducer;
            this.logger = logger;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Validate supported language 
            // For simplicity, message key corresponds to selected language
            var validationPassed = false;
            var message = (Message<int, string>)sourceMessage;
            if (languageStore.ContainsKey(message.Key))
            {
                validationPassed = true;
            }
            else
            {
                var errorMessage = $"No language corresponds to message key '{message.Key}'";
                validationErrorProducer.ProduceEvent(
                    new Confluent.Kafka.Message<int, string>
                    {
                        Key = message.Key,
                        Value = errorMessage
                    });
                logger.LogInformation($"Validation handler: {errorMessage}");
                return null;
            }

            // Call next handler
            var sinkMessage = new Message<int, string>(message.Key, message.Value);
            if (validationPassed)
            {
                logger.LogInformation($"Validation handler: Passed { sinkMessage.Key } { sinkMessage.Value }");
            }
            return await base.HandleMessage(sinkMessage);
        }
    }
}
