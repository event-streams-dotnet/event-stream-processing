using System.Threading.Tasks;

namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Message handler.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Set next handler.
        /// </summary>
        /// <param name="nextHandler">Next handler.</param>
        void SetNextHandler(IMessageHandler nextHandler);

        /// <summary>
        /// Handle message.
        /// </summary>
        /// <param name="sourceMessage">Source message.</param>
        /// <returns>Task which will complete when message is handled.</returns>
        Task<Message> HandleMessage(Message sourceMessage);
    }
}
