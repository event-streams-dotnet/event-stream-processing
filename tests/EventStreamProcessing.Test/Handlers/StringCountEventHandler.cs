using EventStreamProcessing.Abstractions;
using EventStreamProcessing.Test.Mocks;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Handlers
{
    public class StringCountEventHandler : MessageHandler
    {
        private readonly IEventProducer<int> messageHandledProducer;

        public StringCountEventHandler(IEventProducer<int> messageHandledProducer)
        {
            this.messageHandledProducer = messageHandledProducer;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Get count
            var count = ((MockMessage<string>)sourceMessage).Value.Length;

            // Notify subscribers
            messageHandledProducer.ProduceEvent(count);

            // Call next handler
            var sinkMessage = new MockMessage<int>(count);
            return await base.HandleMessage(sinkMessage);
        }
    }
}
