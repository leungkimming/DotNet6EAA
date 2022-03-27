using Common.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Service.Users;
using API.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger
            , UserService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet(Name = "GetUserList")]
        [AccessCodeAuthorize("AA01")]
        public async Task<IActionResult> Get([FromQuery] GetUserRequest request)
        {
            var users = await _service.SearchAsync(request);
            return Ok(users);
        }

        [HttpPost(Name = "AddNewUser")]
        [AccessCodeAuthorize("AB01")]
        public async Task<IActionResult> Add([FromBody] AddUserRequest request)
        {
            AddUserResponse response;
            response = await _service.AddNewAsync(request);
            return Ok(response);
        }

        [HttpPost("Addpayslip")]
        [AccessCodeAuthorize("AC01")]
        public async Task<IActionResult> AddPayslip([FromBody] AddPayslipRequest request)
        {
            AddPayslipResponse _response;
            _response = await _service.AddUserPayslipAsync(request);
            return Ok(_response);
        }

        [HttpGet("GetPayslip")]
        [AccessCodeAuthorize("AA01")]
        public async Task<IActionResult> GetPayslip([FromQuery] GetPayslipRequest request)
        {
            var payslips = await _service.SearchAsync(request);
            return Ok(payslips);
        }
    }
}