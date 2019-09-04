namespace Microsoft.AspNetCore.Builder
{
    using NLogWithKafkaDemo.Middlewares;

    public static class RequestLogServiceCollectionExtensions
    {
        public static IApplicationBuilder UseRequestLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogMiddleware>();
        }
    }
}
