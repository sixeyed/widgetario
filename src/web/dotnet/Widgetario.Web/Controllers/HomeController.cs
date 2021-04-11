using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Widgetario.Web.Models;
using Widgetario.Web.Services;
using OpenTracing.Util;
using OpenTracing;

namespace Widgetario.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ITracer _tracer;
        private readonly ILogger<HomeController> _logger;
        private readonly ProductService _productsService;
        private readonly StockService _stockService;

        public HomeController(ProductService productsService, StockService stockService, ITracer tracer, IConfiguration config, ILogger<HomeController> logger)
        {
            _productsService = productsService;
            _stockService = stockService;
            _tracer = tracer;
            _config = config;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug($"Loading products & stock");
            var model = new ProductViewModel();
            using (var loadScope = _tracer.BuildSpan("api-load").StartActive())
            {                
                using (var productLoadScope = _tracer.BuildSpan("product-api-load").StartActive())
                {
                    model.Products = await _productsService.GetProducts();
                }                
                foreach (var product in model.Products)
                {
                    using (var stockLoadScope = _tracer.BuildSpan("stock-api-load").StartActive())
                    {
                        var productStock = await _stockService.GetStock(product.Id);
                        product.Stock = productStock.Stock;                        
                    }
                }
                _logger.LogDebug($"Products & stock load took: {stopwatch.Elapsed.TotalMilliseconds}ms");
            }           

            if (_config.GetValue<bool>("Widgetario:Debug"))
            {
                ViewData["Environment"] = $"{_config["Widgetario:Environment"]} @ {Dns.GetHostName()}";
            }
            else
            {
                ViewData["Environment"] = $"{_config["Widgetario:Environment"]}";
            }

            ViewData["Theme"] = _config.GetValue<string>("Widgetario:Theme") ?? "light";

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
