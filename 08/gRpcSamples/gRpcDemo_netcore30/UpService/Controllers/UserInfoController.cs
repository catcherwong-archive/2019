namespace UpService.Controllers
{
    using Grpc.Net.Client;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Threading;

    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {       
        private readonly ILogger<UserInfoController> _logger;

        public UserInfoController(ILogger<UserInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> GetAsync(CancellationToken cancellationToken)
        {
            // fixed https error
            var httpclientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true
            };

            var httpClient = new HttpClient(httpclientHandler)
            {
                // The port number(5001) must match the port of the gRPC server.
                BaseAddress = new Uri("https://localhost:5001")
            };

            try
            {
                var client = GrpcClient.Create<UserInfoRpcService.UserInfoRpcServiceClient>(httpClient);

                var callOptions = new Grpc.Core.CallOptions()
                    // StatusCode=Cancelled
                    .WithCancellationToken(cancellationToken)
                    // StatusCode=DeadlineExceeded
                    .WithDeadline(DateTime.UtcNow.AddMilliseconds(200));

                var reply = await client.GetByIdAsync(new GetUserByIdRequest { Id = 1 }, callOptions);

                return reply.Name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "some exception occure");

                return "error";
            }            
        }
    }
}
