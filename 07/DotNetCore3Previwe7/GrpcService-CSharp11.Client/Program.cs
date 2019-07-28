namespace GrpcService_CSharp11.Client
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Grpc.Net.Client;
    using Greet;

    class Program
    {
        static async Task Main(string[] args)
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

            var client = GrpcClient.Create<Greeter.GreeterClient>(httpClient);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
