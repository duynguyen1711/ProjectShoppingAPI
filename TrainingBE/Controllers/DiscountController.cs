using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Discount>> GetAllDiscounts()
        {
            var discounts = _discountService.GetAllDiscounts();
            return Ok(discounts);
        }
        [HttpGet("{id}")]
        public ActionResult<Discount> GetDiscountById(int id)
        {
            var discount = _discountService.GetDiscountById(id);
            if (discount == null)
            {
                return NotFound("Discount not found");
            }

            return Ok(discount);
        }
        [HttpPost]
        public ActionResult AddDiscount(Discount discount)
        {
            string errorMessage;
            if (!_discountService.ValidateAddDisscount(discount, out errorMessage))
            {
                return BadRequest(errorMessage);
            }
            try
            {
                _discountService.AddDiscount(discount);
                return Ok("Discount added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateDiscount(int id, Discount discount)
        {
            string errorMessage;
            if (_discountService.UpdateDiscount(id, discount, out errorMessage))
            {
                return Ok("Discount updated successfully.");
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteDiscount(int id)
        {
            string errorMessage;
            _discountService.DeleteDiscount(id, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }

            return Ok("Discount deleted successfully.");
        }
    }
}
