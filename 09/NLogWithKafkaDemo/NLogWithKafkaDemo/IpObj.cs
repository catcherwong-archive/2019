namespace NLogWithKafkaDemo
{
    using System;

    internal class IpObj
    {
        public string Ip { get; set; }

        public DateTimeOffset Expiration { get; set; }
    }
}
