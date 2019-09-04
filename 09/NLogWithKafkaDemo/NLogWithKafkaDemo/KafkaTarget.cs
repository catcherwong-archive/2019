namespace NLogWithKafkaDemo
{
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using NLog;
    using NLog.Common;
    using NLog.Config;
    using NLog.Layouts;
    using NLog.Targets;
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Threading;

    [Target("Kafka")]
    public class KafkaTarget : TargetWithLayout
    {
        private const char SplitChar = '~';
        private readonly ConcurrentQueue<IProducer<Null, string>> _producerPool;
        private int _pCount;
        private int _maxSize;

        public KafkaTarget()
        {
            _producerPool = new ConcurrentQueue<IProducer<Null, string>>();
            _maxSize = 10;
        }

        [RequiredParameter]
        public Layout Topic { get; set; }

        [RequiredParameter]
        public string BootstrapServers { get; set; }

        protected override void CloseTarget()
        {
            base.CloseTarget();
            _maxSize = 0;
            while (_producerPool.TryDequeue(out var context))
            {
                context.Dispose();
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {            
            var instanceIp = GetCurrentIp();
            string topic = this.Topic.Render(logEvent);
            string logMessage = this.Layout.Render(logEvent);

            var (traceId, requestIp, msg) = GetInfo(logMessage);

            var json = JsonConvert.SerializeObject(new
            {
                dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                level = logEvent.Level.Name.ToUpper(),
                instanceIp = instanceIp,
                traceId = traceId,
                requestIp = requestIp,
                @class = logEvent.LoggerName,
                message = msg
            });

            var producer = RentProducer();

            try
            {
                var result = producer.ProduceAsync(topic, new Message<Null, string>()
                {
                    Value = json
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, $"kafka published error.");
            }
            finally
            {
                var returned = Return(producer);
                if (!returned)
                {
                    producer.Dispose();
                }
            }
           
            base.Write(logEvent);
        }

        private IProducer<Null, string> RentProducer()
        {
            if (_producerPool.TryDequeue(out var producer))
            {
                Interlocked.Decrement(ref _pCount);

                return producer;
            }

            var config = new ProducerConfig
            {
                BootstrapServers = BootstrapServers,
            };

            producer = new ProducerBuilder<Null, string>(config).Build();

            return producer;
        }

        private bool Return(IProducer<Null, string> producer)
        {
            if (Interlocked.Increment(ref _pCount) <= _maxSize)
            {
                _producerPool.Enqueue(producer);

                return true;
            }

            Interlocked.Decrement(ref _pCount);

            return false;
        }

        private string GetCurrentIp()
        {
            var instanceIp = "127.0.0.1";

            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ipAddr in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (ipAddr.AddressFamily.ToString() == "InterNetwork")
                    {
                        instanceIp = ipAddr.ToString();
                        break;
                    }
                }
            }
            catch
            {
            }

            return instanceIp;
        }

        private (string traceId, string requestIp, string msg) GetInfo(string logMessage)
        {
            var span = logMessage.AsSpan();

            var fIndex = span.IndexOf(SplitChar);
            var lIndex = span.LastIndexOf(SplitChar);

            var traceId = fIndex == 0 ? string.Empty : span.Slice(0, fIndex - 1).ToString();
            var requestIp = lIndex == 1 ? string.Empty : span.Slice(fIndex + 1, lIndex - fIndex - 1).ToString();
            var msg = span.Slice(lIndex + 1).ToString();

            return (traceId, requestIp, msg);
        }
    }
}
