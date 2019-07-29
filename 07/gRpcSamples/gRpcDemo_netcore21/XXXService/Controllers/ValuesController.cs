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
            var res = await _service.GetListAsync(0,"");

            return res?.Select(x=>x.Name)?.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> GetAsync(int id)
        {
            var res = await _service.GetByIdAsync(id);

            return res?.Name ?? "none";
        }
    }
}
