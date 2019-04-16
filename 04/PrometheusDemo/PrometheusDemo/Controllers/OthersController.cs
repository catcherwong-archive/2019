namespace PrometheusDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Route("api/others")]
    public class OthersController : ControllerBase
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            if(new System.Random().NextDouble() < 0.5)
            {
                throw new System.Exception("test exception");
            }

            return new string[] { "value1", "value2" };
        }       
    }
}
