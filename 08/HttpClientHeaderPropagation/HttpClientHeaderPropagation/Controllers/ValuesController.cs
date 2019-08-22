namespace HttpClientHeaderPropagation.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ValuesController(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }


        // GET api/values
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var traceId = string.Empty;

            if (Request.Headers.TryGetValue("traceId", out var tId))
            {
                traceId = tId.ToString();
                Console.WriteLine($"{traceId} from request {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.");
            }
            else
            {
                traceId = System.Guid.NewGuid().ToString("N");
                Request.Headers.Add("traceId", new Microsoft.Extensions.Primitives.StringValues(traceId));
                Console.WriteLine($"{traceId} from generated {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.");
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("traceId", traceId);

                var res = await client.GetAsync("http://localhost:9898/api/values/demo1");
                var str = await res.Content.ReadAsStringAsync();
                Console.WriteLine($"{traceId} demo1 return {str} at {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
                return str;
            }


            //var client = _clientFactory.CreateClient("demo1");
            //var res = await client.GetAsync("http://localhost:9898/api/values/demo1");
            //var str = await res.Content.ReadAsStringAsync();

            //var traceId = string.Empty;
            //if (Request.Headers.TryGetValue("traceId", out var tId)) traceId = tId.ToString();
            //Console.WriteLine($"{traceId} demo1 return {str} at {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");

            //return str;

        }

        // GET api/values/demo1
        [HttpGet("demo1")]
        public async Task<string> GetDemo1()
        {
            var client = _clientFactory.CreateClient("demo2");
            var res = await client.GetAsync("http://localhost:9898/api/values/demo2");
            var str = await res.Content.ReadAsStringAsync();            
            return str;
        }

        // GET api/values/demo2
        [HttpGet("demo2")]
        public async Task<string> GetDemo2()
        {
            var client = _clientFactory.CreateClient("demo3");
            var res = await client.GetAsync("http://localhost:9898/api/values/demo3");
            var str = await res.Content.ReadAsStringAsync();
            return str;
        }

        // GET api/values/demo3
        [HttpGet("demo3")]
        public ActionResult<string> GetDemo3()
        {
            return "demo3";
        }

        // GET api/values/demo4
        [HttpGet("demo4")]
        public async Task<string> GetDemo4()
        {
            var client = _clientFactory.CreateClient("demo3");
            var res = await client.GetAsync("http://localhost:9898/api/values/demo3");
            var str = await res.Content.ReadAsStringAsync();

            var traceId = string.Empty;
            if (Request.Headers.TryGetValue("traceId", out var tId)) traceId = tId.ToString();
            Console.WriteLine($"{traceId} demo3 return {str} at {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");

            return str;
        }
    }
}
