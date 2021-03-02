using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Widgetario.ProductApi.Entities;

namespace Widgetario.ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductContext context, IConfiguration config, ILogger<ProductsController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _context.Products.ToArrayAsync();
            var priceFactor = _config.GetValue<double>("Price:Factor");
            var updatedProducts= products.Select(p => { p.Price *= priceFactor; return p; }).ToList();
            return Ok(products);
        }
    }
}
