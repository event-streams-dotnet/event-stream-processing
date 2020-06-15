using System.Threading;

namespace EventStreamProcessing.Abstractions
{
    public interface IEventConsumer<TSourceEvent>
    {
        TSourceEvent ConsumeEvent(CancellationToken cancellationToken = default);
    }
}
