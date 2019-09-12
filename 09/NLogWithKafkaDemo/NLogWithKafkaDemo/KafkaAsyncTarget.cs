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
    using System.Threading.Tasks;

    [Target("KafkaAsync")]
    public class KafkaAsyncTarget : AsyncTaskTarget
    {
        // Pooling
        private readonly ConcurrentQueue<IProducer<Null, string>> _producerPool;
        private int _pCount;
        private int _maxSize;
        private ConcurrentDictionary<string, IpObj> _cache;

        private const string IP_CACHE_KEY = "memory:ipaddress";

        public KafkaAsyncTarget()
        {
            _producerPool = new ConcurrentQueue<IProducer<Null, string>>();
            _maxSize = 10;
            _cache = new ConcurrentDictionary<string, IpObj>();
        }

        [RequiredParameter]
        public Layout Topic { get; set; }

        [RequiredParameter]
        public string BootstrapServers { get; set; }

        [RequiredParameter]
        public Layout TraceId { get; set; }

        [RequiredParameter]
        public Layout RequestIp { get; set; }

        protected override void CloseTarget()
        {
            base.CloseTarget();
            _maxSize = 0;
            while (_producerPool.TryDequeue(out var context))
            {
                context.Dispose();
            }
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

        private string GetCurrentIpFromCache()
        {
            if (_cache.TryGetValue(IP_CACHE_KEY, out var obj))
            {
                return DateTimeOffset.UtcNow.Subtract(obj.Expiration) < TimeSpan.Zero
                                    ? obj.Ip
                                    : BuildCacheAndReturnIp();
            }
            else
            {
                return BuildCacheAndReturnIp();
            }
        }

        private string BuildCacheAndReturnIp()
        {
            var newObj = new IpObj
            {
                Ip = GetCurrentIp(),
                Expiration = DateTimeOffset.UtcNow.AddMinutes(5),
            };

            _cache.AddOrUpdate(IP_CACHE_KEY, newObj, (x, y) => newObj);

            return newObj.Ip;
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

        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            var instanceIp = GetCurrentIpFromCache();

            string topic = base.RenderLogEvent(this.Topic, logEvent);
            string traceId = base.RenderLogEvent(this.TraceId, logEvent);
            string requestIp = base.RenderLogEvent(this.RequestIp, logEvent);
            string msg = base.RenderLogEvent(this.Layout, logEvent);

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
                await producer.ProduceAsync(topic, new Message<Null, string>()
                {
                    Value = json
                });
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
        }
    }
}
