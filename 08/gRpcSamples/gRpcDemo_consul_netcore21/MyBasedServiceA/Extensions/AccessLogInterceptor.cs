namespace MyBasedServiceA
{
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class AccessLogInterceptor : Interceptor
    {
        private readonly Microsoft.Extensions.Logging.ILogger _accessLogger;        

        public AccessLogInterceptor(ILoggerFactory loggerFactory)
        {
            _accessLogger = loggerFactory.CreateLogger<AccessLogInterceptor>();
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            TResponse response = null;
            Exception exception = null;
            try
            {
                response = await continuation(request, context);
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                sw.Stop();
                var log = new 
                {
                    begint_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ms"),
                    cost =  sw.ElapsedMilliseconds,
                    interface_name = context.Method,
                    request_content = request,
                    response_content = response,
                    source_ip = context.Peer,                    
                    ex = exception?.Message
                };

                _accessLogger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(log));   
            }            
        }
    }
}
