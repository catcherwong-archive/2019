namespace PrometheusDemo
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Prometheus;
    using System;
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
        
        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;

            var counter = Metrics.CreateCounter("prometheus_demo_request_total", "HTTP Requests Total", new CounterConfiguration
            {
                LabelNames = new[] { "path", "method", "status" }
            });

            var statusCode = 200;

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception)
            {
                statusCode = 500;
                counter.Labels(path, method, statusCode.ToString()).Inc();

                throw;
            }
            
            if (path != "/metrics")
            {
                statusCode = httpContext.Response.StatusCode;
                counter.Labels(path, method, statusCode.ToString()).Inc();
            }
        }
    }
}
