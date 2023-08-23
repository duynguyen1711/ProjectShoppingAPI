using Microsoft.AspNetCore.Mvc;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController: ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;


        public EmailController(IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDTO request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
        [HttpPost("send-emails-to-users")]
        public IActionResult SendEmailsToUsers()
        {
            try
            {
                List<User> users = _unitOfWork.UserRepository.GetAll().ToList(); // Lấy danh sách người dùng từ cơ sở dữ liệu
                _emailService.SendEmailsToUsers(users);

                return Ok("Emails sent to all users successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
