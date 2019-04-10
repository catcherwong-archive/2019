namespace PrometheusDemo
{
    using Microsoft.AspNetCore.Builder;

    public static class RequestMiddlewareExtensions
    {        
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
