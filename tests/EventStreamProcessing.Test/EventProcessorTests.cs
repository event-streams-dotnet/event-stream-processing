using EventStreamProcessing.Abstractions;
using EventStreamProcessing.Kafka;
using EventStreamProcessing.Test.Handlers;
using EventStreamProcessing.Test.Mocks;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EventStreamProcessing.Test
{
    public class EventProcessorTests
    {
        [Fact]
        public async Task EventProcessor_Should_Process_Events()
        {
            // Arrange
            var consumerMock = new Mock<IEventConsumer<string>>();
            consumerMock.Setup(c => c.ConsumeEvent(default)).Returns("Hello World");
            var producerMock = new Mock<IEventProducer<int>>();

            var lowerHandledProducer = new MessageHandledProducer<string>();
            var reverseHandledProducer = new MessageHandledProducer<string>();
            var countHandledProducer = new MessageHandledProducer<int>();

            var lowerHandler = new StringLowerEventHandler(lowerHandledProducer);
            var reverseHandler = new StringReverseEventHandler(reverseHandledProducer);
            var countHandler = new StringCountEventHandler(countHandledProducer);
            var handlers = new List<IMessageHandler>
            {
                lowerHandler,
                reverseHandler,
                countHandler
            };

            var processor = new MockEventProcessor<string, int>(
                consumerMock.Object, producerMock.Object, handlers.ToArray());

            // Act
            await processor.Process();

            // Assert
            producerMock.Verify(p => p.ProduceEvent(It.IsAny<int>()));
            Assert.Equal("hello world", lowerHandledProducer.SinkEvent);
            Assert.Equal("dlrow olleh", reverseHandledProducer.SinkEvent);
            Assert.Equal(11, countHandledProducer.SinkEvent);
        }

        [Fact]
        public async Task Kafka_EventProcessor_Should_Process_Events()
        {
            // Arrange
            var expectedDeliveryResult = new Confluent.Kafka.DeliveryResult<int, int>
                { Message = new Confluent.Kafka.Message<int, int> { Key = 1, Value = 11 } };
            var consumerMock = new Mock<IEventConsumer<Confluent.Kafka.Message<int, string>>>();
            consumerMock.Setup(c => c.ConsumeEvent(default))
                .Returns(new Confluent.Kafka.Message<int, string> { Key = 1, Value = "Hello World" });
            var producerMock = new Mock<IEventProducerAsync<Confluent.Kafka.Message<int, int>, Confluent.Kafka.DeliveryResult<int, int>>>();
            producerMock.Setup(p => p.ProduceEventAsync(It.IsAny<Confluent.Kafka.Message<int, int>>()))
                .ReturnsAsync(expectedDeliveryResult);

            var lowerHandledProducer = new MessageHandledProducer<string>();
            var reverseHandledProducer = new MessageHandledProducer<string>();
            var countHandledProducer = new MessageHandledProducer<int>();

            var lowerHandler = new KafkaLowerEventHandler(lowerHandledProducer);
            var reverseHandler = new KafkaReverseEventHandler(reverseHandledProducer);
            var countHandler = new KafkaCountEventHandler(countHandledProducer);
            var handlers = new List<IMessageHandler>
            {
                lowerHandler,
                reverseHandler,
                countHandler
            };

            var processor = new KafkaEventProcessorWithResult<int, string, int, int>(
                consumerMock.Object, producerMock.Object, handlers.ToArray());

            // Act
            var deliveryResult = await processor.ProcessWithResult();

            // Assert
            producerMock.Verify(p => p.ProduceEventAsync(It.IsAny<Confluent.Kafka.Message<int, int>>()));
            Assert.Equal(expectedDeliveryResult.Key, deliveryResult.Key);
            Assert.Equal(expectedDeliveryResult.Value, deliveryResult.Value);
            Assert.Equal("hello world", lowerHandledProducer.SinkEvent);
            Assert.Equal("dlrow olleh", reverseHandledProducer.SinkEvent);
            Assert.Equal(11, countHandledProducer.SinkEvent);
        }
    }
}
