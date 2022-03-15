using Common.DTOs.Users;
using Common.Shared;
using Microsoft.AspNetCore.Mvc;
using Service.Users;

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
        public async Task<IActionResult> Get([FromQuery] GetUserRequest request)
        {
            var users = await _service.SearchAsync(request);
            return Ok(users);
        }

        [HttpPost(Name = "AddNewUser")]
        public async Task<IActionResult> Add([FromBody] AddUserRequest request)
        {
            AddUserResponse response;
            try
            {
                response = await _service.AddNewAsync(request);
            } catch (UserAlreadyExistException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("Addpayslip")]
        public async Task<IActionResult> AddPayslip([FromBody] AddPayslipRequest request)
        {
            AddPayslipResponse _response;
            try
            {
                _response = await _service.AddUserPayslipAsync(request);
            } catch (PayslipMonthAlreadyExistException ex) {
                return BadRequest(ex.Message);
            }
            return Ok(_response);
        }

        [HttpGet("GetPayslip")]
        public async Task<IActionResult> GetPayslip([FromQuery] GetPayslipRequest request)
        {
            var payslips = await _service.SearchAsync(request);
            return Ok(payslips);
        }
    }
}