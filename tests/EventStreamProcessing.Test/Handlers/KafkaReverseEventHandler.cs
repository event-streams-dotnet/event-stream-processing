using EventStreamProcessing.Abstractions;
using System;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Handlers
{
    public class KafkaReverseEventHandler : MessageHandler
    {
        private readonly IEventProducer<string> messageHandledProducer;

        public KafkaReverseEventHandler(IEventProducer<string> messageHandledProducer)
        {
            this.messageHandledProducer = messageHandledProducer;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Transform message
            var message = (Message<int, string>)sourceMessage;
            var chars = message.Value.ToCharArray();
            Array.Reverse(chars);
            message.Value = new string(chars);

            // Notify subscribers
            messageHandledProducer.ProduceEvent(message.Value);

            // Call next handler
            var sinkMessage = new Message<int, string>(message.Key, message.Value);
            return await base.HandleMessage(sinkMessage);
        }
    }
}
