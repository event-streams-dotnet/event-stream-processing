using EventStreamProcessing.Abstractions;

namespace EventStreamProcessing.Test.Mocks
{
    public class MessageHandledProducer<TSinkEvent> : IEventProducer<TSinkEvent>
    {
        public TSinkEvent SinkEvent { get; set; }

        public void ProduceEvent(TSinkEvent sinkEvent)
        {
            SinkEvent = sinkEvent;
        }
    }
}
