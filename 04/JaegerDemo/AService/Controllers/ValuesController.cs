namespace AService.Controllers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

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
            var res = await GetDemoAsync();
            return res;
        }

        private async Task<string> GetDemoAsync()
        {
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://bservice/api/values")
            };

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            return body;
        }
    }
}
