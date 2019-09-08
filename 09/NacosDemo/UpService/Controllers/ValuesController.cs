namespace UpService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Nacos.AspNetCore;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly INacosServerManager _serverManager;
        private readonly IHttpClientFactory _clientFactory;

        public ValuesController(INacosServerManager serverManager, IHttpClientFactory clientFactory)
        {
            _serverManager = serverManager;
            _clientFactory = clientFactory;
        }

        // GET api/values
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var result = await GetResultAsync();

            if (string.IsNullOrWhiteSpace(result))
            {
                result = "ERROR!!!";
            }

            return result;
        }

        private async Task<string> GetResultAsync()
        {
            var baseUrl = await _serverManager.GetServerAsync("BaseService");

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return "";
            }

            var url = $"{baseUrl}/api/values";

            var client = _clientFactory.CreateClient();

            var result = await client.GetAsync(url);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
