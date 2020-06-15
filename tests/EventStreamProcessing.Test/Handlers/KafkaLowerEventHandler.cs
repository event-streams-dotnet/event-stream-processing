using EventStreamProcessing.Abstractions;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Handlers
{
    public class KafkaLowerEventHandler : MessageHandler
    {
        private readonly IEventProducer<string> messageHandledProducer;

        public KafkaLowerEventHandler(IEventProducer<string> messageHandledProducer)
        {
            this.messageHandledProducer = messageHandledProducer;
        }

        public override async Task<Message> HandleMessage(Message sourceMessage)
        {
            // Transform message
            var message = (Message<int, string>)sourceMessage;
            message.Value = message.Value.ToLower();

            // Notify subscribers
            messageHandledProducer.ProduceEvent(message.Value);

            // Call next handler
            var sinkMessage = new Message<int, string>(message.Key, message.Value);
            return await base.HandleMessage(sinkMessage);
        }
    }
}
