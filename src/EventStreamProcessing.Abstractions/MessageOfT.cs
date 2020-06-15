namespace EventStreamProcessing.Abstractions
{
    public class Message<TKey, TValue> : Message
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Message(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
