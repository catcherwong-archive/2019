namespace XXXService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IABasedService _service;

        public ValuesController(IABasedService service)
        {
            this._service = service;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            var (l, msg) = await _service.GetListAsync(0, "");

            if (!string.IsNullOrWhiteSpace(msg))
            {
                return l?.Select(x => x.Name)?.ToArray();
            }
            else
            {
                return new string[] { msg };
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> GetAsync(int id)
        {
            var (i, msg) = await _service.GetByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(msg))
            {
                return msg;
            }
            else
            {
                return i?.Name ?? "none";
            }
        }
    }
}
