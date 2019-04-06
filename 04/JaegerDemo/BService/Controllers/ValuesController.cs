namespace BService.Controllers
{
    using System;
    using System.Threading.Tasks;
    using EasyCaching.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEasyCachingProviderFactory _providerFactory;
        private readonly BDbContext _dbContext;

        public ValuesController(
            IEasyCachingProviderFactory providerFactory
            , BDbContext dbContext)
        {
            this._providerFactory = providerFactory;
            this._dbContext = dbContext;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var provider = _providerFactory.GetCachingProvider("m1");

            var obj = await provider.GetAsync("mykey", async () => await _dbContext.DemoObjs.ToListAsync(), TimeSpan.FromSeconds(30));

            return Ok(obj);
        }
    }
}
