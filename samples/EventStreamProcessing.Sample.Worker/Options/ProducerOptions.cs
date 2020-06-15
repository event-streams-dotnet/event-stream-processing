namespace EventStreamProcessing.Sample.Worker.Options
{
    public class ProducerOptions
    {
        public string Brokers { get; set; }
        public string ValidationTopic { get; set; }
        public string FinalTopic { get; set; }
    }
}
