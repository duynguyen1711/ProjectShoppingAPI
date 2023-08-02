using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model = _paymentService.GetPayment();
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
            var model = _paymentService.GetPaymentById(id);
            if (model == null)
            {
                return NotFound("Not Found");
            }
            return Ok(model);
        }
        [HttpDelete("{id}")]

        public ActionResult DeletePayment(int id)
        {

            string errorMessage;
            _paymentService.DeletePayment(id, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }

            return Ok("Payment deleted successfully.");
        }
        [HttpPost]
        public ActionResult AddPayment(Payment payment)
        {
             _paymentService.AddPayment(payment);
             return Ok("Payment added successfully."); 
        }
    }
}
