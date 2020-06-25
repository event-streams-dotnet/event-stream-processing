using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Async event producer.
    /// </summary>
    /// <typeparam name="TSinkEvent">Sink event type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    public interface IEventProducerAsync<TSinkEvent, TResult>
    {
        /// <summary>
        /// Produce event
        /// </summary>
        /// <param name="sinkEvent">Sink event.</param>
        /// <returns>Task which will complete when Produce finishes.</returns>
        Task<TResult> ProduceEventAsync(TSinkEvent sinkEvent);
    }
}
