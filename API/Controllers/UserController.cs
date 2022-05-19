using Common;
using Microsoft.AspNetCore.Mvc;
using Service;
using Data;
using System.Net;
using Business;

namespace API {
    [ApiController]
    [Route("users")]
    [AutoValidateAntiforgeryToken]
    public class UserController : ControllerBase {
        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;
        private readonly IPaymentQuery _paymentQuery;
        private readonly IDepartmentQuery _departmentQuery;

        public UserController(ILogger<UserController> logger
            , UserService service, IPaymentQuery paymentquery,
            IDepartmentQuery _departmentquery) {
            _service = service;
            _logger = logger;
            _paymentQuery = paymentquery;
            _departmentQuery = _departmentquery;
        }

        [HttpGet(Name = "GetUserList")]
        [AccessCodeAuthorize("AA01")]
        public async Task<IActionResult> Get([FromQuery] GetUserRequestV1 request) {
            var users = await _service.SearchAsyncV1(request);
            return Ok(users);
        }

        [HttpPost]
        [Route("getuserlist")]
        [AccessCodeAuthorize("AA01")]
        public async Task<IActionResult> Get([FromBody] GetUserRequest request) {
            GetAllDatasResponse<UserInfoDTO> users = await _service.SearchAsync(request);
            return Ok(users);
        }

        [HttpPost(Name = "AddNewUser")]
        [AccessCodeAuthorize("AB01")]
        public async Task<IActionResult> Add([FromBody] AddUserRequest request) {
            AddUserResponse response;
            request.Refresh(HttpContext.User.Identity.Name, DateTime.Now);
            response = await _service.AddNewAsync(request);
            return Ok(response);
        }

        [HttpPost("Addpayslip")]
        [AccessCodeAuthorize("AC01")]
        public async Task<IActionResult> AddPayslip([FromBody] AddPayslipRequest request) {
            AddPayslipResponse _response;
            request.Refresh(HttpContext.User.Identity.Name, DateTime.Now);
            _response = await _service.AddUserPayslipAsync(request);
            return Ok(_response);
        }

        [HttpGet("GetPayslip")]
        [AccessCodeAuthorize("AA01")]
        public async Task<IActionResult> GetPayslip([FromQuery] GetPayslipRequest request) {
            var payslips = await _service.SearchAsync(request);
            return Ok(payslips);
        }
        [HttpGet("GetPaymentStat")]
        [AccessCodeAuthorize("AA01")]
        [ProducesResponseType(typeof(IEnumerable<PaymentSummary>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PaymentSummary>>> GetPaymentOfFrequentWorkers(int days) {
            var paymentSummary = await _paymentQuery.GetPaymentOfFrequentWorkersAsync(days);

            return Ok(paymentSummary);
        }
        [HttpGet("GetDepartmentList")]
        [AccessCodeAuthorize("AA01")]
        [ProducesResponseType(typeof(IEnumerable<Dep>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Dep>>> GetDepartments() {
            IEnumerable<Dep> departmentList = await _departmentQuery.GetDepartmentsAsync();

            return Ok(departmentList);
        }
    }
}