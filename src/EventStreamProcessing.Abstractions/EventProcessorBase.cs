namespace EventStreamProcessing.Abstractions
{
    public abstract class EventProcessorBase
    {
        protected readonly IMessageHandler[] handlers;

        public EventProcessorBase(
            params IMessageHandler[] handlers)
        {
            this.handlers = handlers;
        }

        protected virtual void BuildHandlerChain()
        {
            for (int i = 0; i < handlers.Length - 1; i++)
            {
                handlers[i].SetNextHandler(handlers[i + 1]);
            }
        }
    }
}
