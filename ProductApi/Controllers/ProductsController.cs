using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        //Dependecy Injection 
        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = new List<Product>()
            {
                new Product() {Id = 1, ProductName="Computer" },
                new Product() {Id = 2, ProductName="Keyboard" },
                new Product() {Id = 3, ProductName="Mouse" }
            };
            _logger.LogInformation(" GetAllProducts action has been called.");
            return Ok(products);
        }
    }
}
