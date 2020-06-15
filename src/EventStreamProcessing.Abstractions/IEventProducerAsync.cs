using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public interface IEventProducerAsync<TSinkEvent, TResult>
    {
        Task<TResult> ProduceEventAsync(TSinkEvent sinkEvent);
    }
}
