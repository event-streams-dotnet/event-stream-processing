using EventStreamProcessing.Abstractions;
using EventStreamProcessing.Test.Mocks;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Handlers
{
    public class StringLowerEventHandler : MessageHandler
    {
        private readonly IEventProducer<string> messageHandledProducer;

        public StringLowerEventHandler(IEventProducer<string> messageHandledProducer)
        {
            this.messageHandledProducer = messageHandledProducer;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Transform message
            var message = ((MockMessage<string>)sourceMessage).Value.ToLower();

            // Notify subscribers
            messageHandledProducer.ProduceEvent(message);

            // Call next handler
            var sinkMessage = new MockMessage<string>(message);
            return await base.HandleMessage(sinkMessage);
        }
    }
}
