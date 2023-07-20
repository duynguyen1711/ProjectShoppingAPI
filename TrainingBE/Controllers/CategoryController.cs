using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categorytService;

        public CategoryController(ICategoryService categoryService)
        {
            _categorytService = categoryService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model = _categorytService.GetCategory();
            return Ok(model);
        }
    }
}
