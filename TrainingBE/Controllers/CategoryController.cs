using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model = _categoryService.GetCategory();
            return Ok(model);
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID. ID must be a non-negative number.");
            }
            var model = _categoryService.GetCategoryById(id);
            if (model == null)
            {
                return NotFound("Not Found");
            }
            return Ok(model);
        }
        [HttpDelete("{id}")]
       
        public ActionResult DeleteCategory(int id)
        {

            string errorMessage;
            _categoryService.DeleteCategory(id, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }

            return Ok("Category deleted successfully.");
        }
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            string errorMessage;
            if (!_categoryService.ValidateAddCategory(category, out errorMessage))
            {
                return BadRequest(errorMessage);
            }
            try
            {
                _categoryService.AddCategory(category);
                return Ok("Category added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, Category updatedCategory)
        {
            string errorMessage;
            if (_categoryService.UpdateCategory(id, updatedCategory, out errorMessage))
            {
                return Ok("Category updated successfully.");
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }
    }
}
