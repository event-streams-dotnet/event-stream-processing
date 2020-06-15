namespace EventStreamProcessing.Abstractions
{
    public interface IEventProducer<TSinkEvent>
    {
        void ProduceEvent(TSinkEvent sinkEvent);
    }
}
