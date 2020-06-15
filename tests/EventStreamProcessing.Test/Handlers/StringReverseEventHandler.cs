using EventStreamProcessing.Abstractions;
using EventStreamProcessing.Test.Mocks;
using System;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Handlers
{
    public class StringReverseEventHandler : MessageHandler
    {
        private readonly IEventProducer<string> messageHandledProducer;

        public StringReverseEventHandler(IEventProducer<string> messageHandledProducer)
        {
            this.messageHandledProducer = messageHandledProducer;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Transform message
            var message = ((MockMessage<string>)sourceMessage).Value;
            var chars = message.ToCharArray();
            Array.Reverse(chars);
            message = new string(chars);

            // Notify subscribers
            messageHandledProducer.ProduceEvent(message);

            // Call next handler
            var sinkMessage = new MockMessage<string>(message);
            return await base.HandleMessage(sinkMessage);
        }
    }
}
