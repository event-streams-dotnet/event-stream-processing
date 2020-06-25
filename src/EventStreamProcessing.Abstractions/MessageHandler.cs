using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Message handler.
    /// </summary>
    public abstract class MessageHandler : IMessageHandler
    {
        private IMessageHandler nextHandler;

        /// <summary>
        /// Set next handler.
        /// </summary>
        /// <param name="nextHandler">Next handler.</param>
        public void SetNextHandler(IMessageHandler nextHandler)
        {
            this.nextHandler = nextHandler;
        }

        /// <summary>
        /// Handle message.
        /// </summary>
        /// <param name="sourceMessage">Source message.</param>
        /// <returns>Task which will complete when message is handled.</returns>
        public virtual async Task<Message> HandleMessage(Message sourceMessage)
        {
            return nextHandler != null ? await (nextHandler?.HandleMessage(sourceMessage)) : sourceMessage;
        }
    }
}
