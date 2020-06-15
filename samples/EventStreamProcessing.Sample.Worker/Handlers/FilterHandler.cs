using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStreamProcessing.Sample.Worker.Handlers
{
    public class FilterHandler : MessageHandler
    {
        private readonly IDictionary<int, string> languageStore;
        private readonly ILogger logger;

        public FilterHandler(IDictionary<int, string> languageStore,
            ILogger logger)
        {
            this.languageStore = languageStore;
            this.logger = logger;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Filter out message if language not supported
            // For simplicity, message key corresponds to selected language
            var message = (Message<int, string>)sourceMessage;
            if (!languageStore.ContainsKey(message.Key))
            {
                logger.LogInformation($"Filter handler: Rejected { message.Key } { message.Value }");
                return sourceMessage;
            }

            // Call next handler
            var sinkMessage = new Message<int, string>(message.Key, message.Value);
            logger.LogInformation($"Filter handler: Accepted { sinkMessage.Key } { sinkMessage.Value }");
            return await base.HandleMessage(sinkMessage);
        }
    }
}
