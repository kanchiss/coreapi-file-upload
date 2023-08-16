using Microsoft.AspNetCore.Http;
using coreapi_file_upload.Models;
using coreapi_file_upload.Repo;
using Microsoft.AspNetCore.Mvc;


namespace coreapi_file_upload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<ProductController> _logger;
        private readonly IProduct _product;
        private readonly IHttpContextAccessor _http;

        public ProductController(ILogger<ProductController> logger, IProduct product, IHttpContextAccessor http)
        {
            _logger = logger;
            _http = http;
            _product = product;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("getproducts")]
        public Task<IList<Product>> GetProducts()
        {
            return _product.GetProducts();
        }

        [HttpGet("getproductbyid/{id}")]
        public Task<Product> GetProductById([FromRoute] string id)
        {
            return _product.GetProductById(id);
        }

        [HttpPost("addproduct")]
        public Task AddProduct([FromBody] Product product)
        {
            return _product.AddProduct(product);
        }

        [HttpPut("updateproduct")]
        public Task UpdateProduct([FromBody] Product product)
        {
            return _product.UpdateProduct(product);
        }

        [HttpDelete("deleteproduct")]
        public Task DeleteProduct([FromRoute] string id)
        {
            return _product.DeleteProduct(id);
        }

        [HttpPost("attach/{id}")]
        public async Task<string> AttachDoc(string id)
        {
            var file = _http.HttpContext.Request.Form.Files[0];
            var pathToSave = Path.Combine(@"E:\KANCHI\PHOTO");
            var fileName = "hello";
            if (file.Length > 0)
            {
                fileName = fileName + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            await _product.AttachProductImage(id, fileName);
            return fileName;
        }
    }
}