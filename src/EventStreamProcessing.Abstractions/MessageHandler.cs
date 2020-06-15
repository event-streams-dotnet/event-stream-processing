using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    public abstract class MessageHandler : IMessageHandler
    {
        private IMessageHandler nextHandler;

        public void SetNextHandler(IMessageHandler nextHandler)
        {
            this.nextHandler = nextHandler;
        }

        public virtual async Task<Message> HandleMessage(Message sourceMessage)
        {
            return nextHandler != null ? await (nextHandler?.HandleMessage(sourceMessage)) : sourceMessage;
        }
    }
}
