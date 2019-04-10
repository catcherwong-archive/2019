namespace PrometheusDemo
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Prometheus;
    using System.Threading.Tasks;

    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestMiddleware(
            RequestDelegate next
            , ILoggerFactory loggerFactory
            )
        {
            this._next = next;
            this._logger = loggerFactory.CreateLogger<RequestMiddleware>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;

            var counter = Metrics.CreateCounter("prometheus_demo_request_total", "prometheus_demo_request_total", new CounterConfiguration
            {
                LabelNames = new[] { "path", "method" }
            });

            counter.Labels(path, method).Inc();
            counter.Inc();


            await _next(httpContext);

        }
    }
}
