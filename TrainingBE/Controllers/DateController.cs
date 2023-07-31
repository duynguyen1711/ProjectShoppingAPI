using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TrainingBE.Model;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public DateController(IMemoryCache cache)
        {
            _cache = cache;
        }
        [HttpPost("set-date")]
        public IActionResult SetSelectedDate([FromBody] DateInput dateInput)
        {
            // Lưu ngày tháng năm vào cache hoặc database để sử dụng trong ProductController
            // Ở đây tôi sẽ lưu vào cache sử dụng IMemoryCache
            _cache.Set("SelectedDate", dateInput.SelectedDate);

            return Ok("Selected date has been set successfully.");
        }
    }
}
