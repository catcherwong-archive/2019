namespace KafkaPubPreDemo
{
    using Confluent.Kafka;
    using Shared;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var config = new ProducerConfig { BootstrapServers = "kafkatest:9092,kafkatest:9093,kafkatest:9094" };

            var rd = new Random().Next(1, 999999);

            var user = new User { Id = rd, Name = $"catcherwong-{rd}" };

            using (var producer = new ProducerBuilder<Ignore, string>(config).Build())
            {
                var dr = producer.ProduceAsync("my-topic", new Message<Ignore, string>
                {
                     Value = user.ToJsonString(),
                }).GetAwaiter().GetResult();
                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
            }
        }
    }
}
