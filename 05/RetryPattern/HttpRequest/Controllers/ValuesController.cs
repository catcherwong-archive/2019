namespace HttpRequest.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ValuesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET api/values
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var url = $"https://www.c-sharpcorner.com/mytestpagefor404";

            var client = _clientFactory.CreateClient("csharpcorner");            
            var response = await  client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }        
    }
}
