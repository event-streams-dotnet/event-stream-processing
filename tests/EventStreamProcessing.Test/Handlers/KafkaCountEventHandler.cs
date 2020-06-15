using EventStreamProcessing.Abstractions;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Handlers
{
    public class KafkaCountEventHandler : MessageHandler
    {
        private readonly IEventProducer<int> messageHandledProducer;

        public KafkaCountEventHandler(IEventProducer<int> messageHandledProducer)
        {
            this.messageHandledProducer = messageHandledProducer;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Get count
            var message = (Message<int, string>)sourceMessage;
            var count = message.Value.Length;

            // Notify subscribers
            messageHandledProducer.ProduceEvent(count);

            // Call next handler
            var sinkMessage = new Message<int, int>(message.Key, count);
            return await base.HandleMessage(sinkMessage);
        }
    }
}
