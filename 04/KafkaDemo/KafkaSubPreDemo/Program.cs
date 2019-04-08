namespace KafkaSubPreDemo
{
    using Confluent.Kafka;
    using Shared;
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "kafkatest:9092,kafkatest:9093,kafkatest:9094",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AutoCommitIntervalMs = 5000,
            };

            Console.WriteLine("begin now ... ");

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                Console.WriteLine($"consumer name is {c.Name}, member id is {c.MemberId}");

                c.Subscribe("my-topic");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }

        private static void OnMessage(ConsumeResult<Ignore, string> e)
        {
            var msg = e.Value;

            Console.WriteLine($"Read '{msg}' from: {e.TopicPartitionOffset}");

            var user = msg.ToObj<User>();

            Console.WriteLine($"{user.Id},{user.Name}");
        }
    }
}
