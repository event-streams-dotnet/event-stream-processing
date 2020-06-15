using System.Collections.Generic;

namespace EventStreamProcessing.Sample.Worker.Options
{
    public class ConsumerOptions
    {
        public string Brokers { get; set; }
        public List<string> TopicsList { get; set; }
    }
}
