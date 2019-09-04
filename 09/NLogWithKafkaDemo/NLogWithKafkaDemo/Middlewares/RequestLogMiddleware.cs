namespace NLogWithKafkaDemo.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLogMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = Guid.NewGuid().ToString("N");
            var requestIp = GetRequestIp(context);
            
            if (context.Request.Headers.TryGetValue("traceId", out var tId))
            {
                traceId = tId.FirstOrDefault().ToString();
            }

            context.Items.Add("traceId", traceId);
            context.Items.Add("requestIp", requestIp);

            // request log
            var reqMsg = await FormatRequest(context.Request);
            _logger.LogInformation($"Path={context.Request.Path},Method={context.Request.Method},QueryString={context.Request.QueryString.ToString()},Body={reqMsg}");

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                var sp = new Stopwatch();
                sp.Start();
                await _next(context);
                sp.Stop();

                // response log
                var resMsg = await FormatResponse(context.Response);
                _logger.LogInformation($"Cost={sp.ElapsedMilliseconds}ms,Response={resMsg}");
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableRewind();

            request.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return text?.Trim();
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            if (response.HasStarted) return string.Empty;

            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text?.Trim();
        }

        private string GetRequestIp(HttpContext context)
        {
            try
            {
                var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Connection.RemoteIpAddress?.ToString();
                }

                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = context.Request.Headers["REMOTE_ADDR"].FirstOrDefault();
                }

                return ip;
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }
        }
    }
}
