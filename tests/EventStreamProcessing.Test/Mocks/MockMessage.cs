using EventStreamProcessing.Abstractions;

namespace EventStreamProcessing.Test.Mocks
{
    public class MockMessage<TValue> : Message
    {
        public MockMessage(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; set; }
    }
}
