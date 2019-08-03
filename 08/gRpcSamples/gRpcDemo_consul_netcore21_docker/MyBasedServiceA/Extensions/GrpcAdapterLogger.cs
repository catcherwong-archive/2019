namespace MyBasedServiceA
{
    using Microsoft.Extensions.Logging;
    using System;

    public class GrpcAdapterLogger : Grpc.Core.Logging.ILogger
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly string _categoryName;
        private ILogger _logger;

        public GrpcAdapterLogger(ILoggerFactory loggerFactory, string categoryName = "Grpc.Core")
        {
            _loggerFactory = loggerFactory;
            _categoryName = categoryName;
            _logger = _loggerFactory.CreateLogger(categoryName);
        }

        public Grpc.Core.Logging.ILogger ForType<T>()
        {
            var categoryName = typeof(T).FullName;
            if (_categoryName == categoryName)
            {
                return this;
            }
            return new GrpcAdapterLogger(_loggerFactory, categoryName);
        }

        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }

        public void Debug(string format, params object[] formatArgs)
        {
            _logger.LogDebug(format);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Error(string format, params object[] formatArgs)
        {
            _logger.LogError(format);
        }

        public void Error(Exception exception, string message)
        {
            _logger.LogError(exception, message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Info(string format, params object[] formatArgs)
        {
            _logger.LogInformation(format);
        }

        public void Warning(string message)
        {
            _logger.LogWarning(message);
        }

        public void Warning(string format, params object[] formatArgs)
        {
            _logger.LogWarning(format);
        }

        public void Warning(Exception exception, string message)
        {
            _logger.LogWarning(exception, message);
        }
    }
}
