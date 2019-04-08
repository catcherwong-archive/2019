namespace KafkaPubDemo
{
    using Confluent.Kafka;
    using Confluent.Kafka.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Shared;

    class Program
    {
        static void Main(string[] args)
        {
            var config = new Dictionary<string, object>
            {                
                { "bootstrap.servers", "kafkatest:9092,kafkatest:9093,kafkatest:9094" }
            };

            var rd = new Random().Next(1, 999999);

            var user = new User { Id = rd, Name = $"catcherwong-{rd}" };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                var dr = producer.ProduceAsync("my-topic", null, user.ToJsonString()).GetAwaiter().GetResult();                
                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
            }
        }
    }
}
