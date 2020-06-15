using System.Collections.Generic;

namespace EventStreamProcessing.Sample.Consumer
{
    public class ConsumerOptions
    {
        public string Brokers { get; set; }
        public List<string> TopicsList { get; set; }
    }
}
