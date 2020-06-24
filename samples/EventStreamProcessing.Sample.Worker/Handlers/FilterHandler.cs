using EventStreamProcessing.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStreamProcessing.Sample.Worker.Handlers
{
    public class FilterHandler : MessageHandler
    {
        private readonly Func<Message<int, string>, bool> filter;
        private readonly ILogger logger;

        public FilterHandler(Func<Message<int, string>, bool> filter,
            ILogger logger)
        {
            this.filter = filter;
            this.logger = logger;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Filter message
            var message = (Message<int, string>)sourceMessage;
            if (!filter(message))
            {
                logger.LogInformation($"Filter handler: Excluded { message.Key } { message.Value }");
                return null;
            }

            // Call next handler
            var sinkMessage = new Message<int, string>(message.Key, message.Value);
            logger.LogInformation($"Filter handler: Accepted { sinkMessage.Key } { sinkMessage.Value }");
            return await base.HandleMessage(sinkMessage);
        }
    }
}
