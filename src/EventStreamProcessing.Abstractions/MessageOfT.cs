namespace EventStreamProcessing.Abstractions
{
    /// <summary>
    /// Message with key and value.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class Message<TKey, TValue> : Message
    {
        /// <summary>
        /// Message key.
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// Message value.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Message constructor.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public Message(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
