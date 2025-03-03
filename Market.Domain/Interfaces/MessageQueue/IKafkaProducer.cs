using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Interfaces.MessageQueue
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, string key, T value);
    }
}
