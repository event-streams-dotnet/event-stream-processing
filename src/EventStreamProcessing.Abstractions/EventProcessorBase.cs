namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Event stream processor base.
    /// </summary>
    public abstract class EventProcessorBase
    {
        /// <summary>
        /// Message handlers.
        /// </summary>
        protected readonly IMessageHandler[] handlers;

        /// <summary>
        /// Event stream processor base constructor.
        /// </summary>
        /// <param name="handlers">Event handlers.</param>
        public EventProcessorBase(
            params IMessageHandler[] handlers)
        {
            this.handlers = handlers;
        }

        /// <summary>
        /// Build handler chain.
        /// </summary>
        protected virtual void BuildHandlerChain()
        {
            for (int i = 0; i < handlers.Length - 1; i++)
            {
                handlers[i].SetNextHandler(handlers[i + 1]);
            }
        }
    }
}
