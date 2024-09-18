using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.ConsumerApp
{
    internal class EventConsumer : BackgroundService
    {
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;           
        }
        
            
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("test-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(5));

                    _logger.LogInformation($"Received message: {result.Message.Value} at {result.Message.Timestamp.UtcDateTime:O} - {result.TopicPartitionOffset}");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming message");
                }

            }
            return Task.CompletedTask;
        }


    }
}
