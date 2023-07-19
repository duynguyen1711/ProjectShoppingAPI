using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ProductNewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductNewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model =_unitOfWork.ProductRepository.GetAll();
            return Ok(model);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetByID(int id ) {
            var model = _unitOfWork.ProductRepository.GetById(id);
            if (model == null)
            {
                return NotFound("Not Found");
            }
            return Ok(model);
        }
        [HttpPost]
        public ActionResult AddProduct( Product product) {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.ProductRepository.Save();
                return Ok("Add success");
            }
            return BadRequest("Add failed");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetById(id);

            if (existingProduct == null)
            {
                return NotFound(); 
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryID = product.CategoryID;

            _unitOfWork.ProductRepository.Update(existingProduct);
            _unitOfWork.ProductRepository.Save();

            return Ok(existingProduct); 
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var existingProduct = _unitOfWork.ProductRepository.GetById(id);

            if (existingProduct == null)
            {
                return NotFound(); 
            }

            _unitOfWork.ProductRepository.Delete(existingProduct);
            _unitOfWork.ProductRepository.Save();

            return NoContent(); 
        }
    }
}
