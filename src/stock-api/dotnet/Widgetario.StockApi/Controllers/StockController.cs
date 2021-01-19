using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Widgetario.StockApi.Entities;

namespace Widgetario.StockApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly StockContext _context;
        private readonly IEasyCachingProvider _cache;
        private readonly IConfiguration _config;
        private readonly ILogger<StockController> _logger;

        public StockController(StockContext context, IEasyCachingProvider cache, IConfiguration config, ILogger<StockController> logger)
        {
            _context = context;
            _cache = cache;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            Product product = null;
            if (_config.GetValue<bool>("Caching:Enabled"))
            {
                var products = await _cache.GetAsync("StockController__products", async () => await _context.Products.ToArrayAsync(), TimeSpan.FromMinutes(5));
                product = products.Value.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                product = await _context.Products.FindAsync(id);
            }
            return Ok(product);
        }
    }
}
