namespace HttpClientHeaderPropagation.Ext
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class HeadersPropagationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public HeadersPropagationDelegatingHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {            
            var traceId = string.Empty;

            if (_accessor.HttpContext.Request.Headers.TryGetValue("traceId", out var tId))
            {
                traceId = tId.ToString();
                Console.WriteLine($"{traceId} from request {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.");
            }
            else
            {
                traceId = System.Guid.NewGuid().ToString("N");
                _accessor.HttpContext.Request.Headers.Add("traceId", new Microsoft.Extensions.Primitives.StringValues(traceId));
                Console.WriteLine($"{traceId} from generated {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.");
            }

            if (!request.Headers.Contains("trace-id"))
            {
                request.Headers.TryAddWithoutValidation("traceId", traceId);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
