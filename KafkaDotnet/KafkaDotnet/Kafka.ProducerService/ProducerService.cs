using Confluent.Kafka;

namespace Kafka.ProducerService
{
    public class ProducerService
    {
        private readonly ILogger<ProducerService> _logger;

        public ProducerService(ILogger<ProducerService> logger)
        {
            _logger = logger;
        }

        public async Task ProduceAsync(CancellationToken cancellationToken)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All,
                AllowAutoCreateTopics = true
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync(
                    "test-topic",
                    new Message<Null, string>
                    { Value = "Hello, World!" },
                    cancellationToken);
                _logger.LogInformation($"Message delivered to {deliveryResult.TopicPartitionOffset}");
                _logger.LogInformation($"Message delivered to {deliveryResult.Value}, Offset: {deliveryResult.Offset}, Partition: {deliveryResult.Partition}, Topic: {deliveryResult.Topic}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while producing messages.");
            }

            producer.Flush(cancellationToken);
        }
    }
}
