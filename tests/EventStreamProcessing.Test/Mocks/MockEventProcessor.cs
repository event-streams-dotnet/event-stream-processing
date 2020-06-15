using EventStreamProcessing.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamProcessing.Test.Mocks
{
    public class MockEventProcessor<TSourceEvent, TSinkEvent> : EventProcessor<string, int>
    {
        public MockEventProcessor(
            IEventConsumer<string> consumer, 
            IEventProducer<int> producer, 
            params IMessageHandler[] handlers) 
                : base(consumer, producer, handlers)
        {
        }

        public override async Task Process(CancellationToken cancellationToken = default)
        {
            // Build chain of handlers
            BuildHandlerChain();

            // Consume event
            var sourceEvent = consumer.ConsumeEvent(cancellationToken);

            // Invoke handler chain
            var sourceMessage = new MockMessage<string>(sourceEvent);
            var sinkMessage = await handlers[0].HandleMessage(sourceMessage) as MockMessage<int>;

            // Produce event
            var sinkEvent = sinkMessage.Value;
            producer.ProduceEvent(sinkEvent);
        }
    }
}
