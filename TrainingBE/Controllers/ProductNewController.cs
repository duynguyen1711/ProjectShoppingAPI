using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ProductNewController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductNewController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model =_productService.GetAllProducts();
            return Ok(model);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetByID(int id ) {
            if (id <= 0)
            {
                return BadRequest("Invalid ID. ID must be a non-negative number.");
            }
            var model = _productService.GetProductById(id);
            if (model == null)
            {
                return NotFound("Not Found");
            }
            return Ok(model);
        }
        [HttpPost]
        public ActionResult AddProduct( Product product) {
            string errorMessage;
            if (!_productService.ValidateAddProduct(product, out errorMessage))
            {
                return BadRequest(errorMessage);
            }
            try
            {
                _productService.AddProduct(product);
                return Ok("Product added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            string errorMessage;
            if (!_productService.UpdateProduct(id, product, out errorMessage))
            {
                return BadRequest(errorMessage);
            }

            return Ok("Product updated successfully.");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID. ID must be a non-negative number.");
            }
            try
            {
                _productService.DeleteProduct(id);
                return Ok("Product deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
