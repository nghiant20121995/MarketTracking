using Confluent.Kafka;
using Market.Domain.Interfaces.MessageQueue;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Market.Infrastructure.MessageQueue
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConfiguration _configuration;

        public KafkaProducer(IConfiguration configuration) {
            _configuration = configuration;
            var bootstrapServers = _configuration
                .GetRequiredSection("Kafka")
                .GetRequiredSection("BootstrapServers").Value;
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers, // Kafka broker address
                Acks = Acks.Leader // Wait for leader acknowledgment
            };

            _producer = new ProducerBuilder<string, string>(config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();
        }
        public async Task ProduceAsync<T>(string topic, string key, T msg)
        {
            var message = new Message<string, string>
            {
                Key = key,
                Value = JsonSerializer.Serialize(msg)
            };
            var deliveryResult = await _producer.ProduceAsync(topic, message);
        }
    }
}
