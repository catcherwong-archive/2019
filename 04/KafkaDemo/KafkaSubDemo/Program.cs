namespace KafkaSubDemo
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
            var consumerConf = new Dictionary<string, object>
            {
              { "group.id", "test-consumer-group" },
              { "bootstrap.servers", "kafkatest:9092,kafkatest:9093,kafkatest:9094" },
              { "auto.commit.interval.ms", 5000 },
              { "auto.offset.reset", "earliest" }
            };

            Console.WriteLine("begin now ... ");

            using (var consumer = new Consumer<Null, string>(consumerConf, null, new StringDeserializer(Encoding.UTF8)))
            {
                Console.WriteLine($"consumer name is {consumer.Name}, member id is {consumer.MemberId}");

                consumer.OnMessage += OnMessage;

                consumer.OnError += (_, error)
                  => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                  => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe("my-topic");

                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }
        }

        private static void OnMessage(object sender, Message<Null, string> e)
        {
            var msg = e.Value;

            Console.WriteLine($"Read '{msg}' from: {e.TopicPartitionOffset}");

            var user = msg.ToObj<User>();

            Console.WriteLine($"{user.Id},{user.Name}");
        }
    }
}
