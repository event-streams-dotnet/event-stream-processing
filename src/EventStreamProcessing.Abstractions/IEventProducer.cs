namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event producer.
    /// </summary>
    /// <typeparam name="TSinkEvent">Sink event type.</typeparam>
    public interface IEventProducer<TSinkEvent>
    {
        /// <summary>
        /// Produce event.
        /// </summary>
        /// <param name="sinkEvent">Sink event.</param>
        void ProduceEvent(TSinkEvent sinkEvent);
    }
}
