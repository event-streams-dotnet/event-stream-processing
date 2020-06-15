using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public interface IMessageHandler
    {
        void SetNextHandler(IMessageHandler nextHandler);

        Task<Message> HandleMessage(Message sourceMessage);
    }
}
