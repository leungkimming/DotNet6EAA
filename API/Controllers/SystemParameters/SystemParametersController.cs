using Common;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API {
    [ApiController]
    [Route("systemparameters")]
    [AutoValidateAntiforgeryToken]
    public class SystemParametersController : ControllerBase {
        private readonly SystemParametersService _service;
        private readonly ILogger<UserController> _logger;

        public SystemParametersController(ILogger<UserController> logger, SystemParametersService service) {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("searchall")]
        [AccessCodeAuthorize("SP01")]
        public async Task<IActionResult> SearchAll([FromBody]SystemParametersSearchRequest request) {

            GetAllDatasResponse<SystemParametersSearchResponse> systemParametersResponse = await _service.SearchAsync(request);
            return Ok(systemParametersResponse);
        }
        [HttpPost]
        [Route("addsystemparameter")]
        [AccessCodeAuthorize("SP02")]
        public async Task<IActionResult> AddSystemParameter([FromBody]AddSystemParameterRequest request) {
            AddDataResponse response;
            request.Refresh(HttpContext.User.Identity.Name,DateTime.Now);
            response = await _service.AddSystemParameterAsync(request);
            return Ok(response);
        }
        [HttpPost]
        [Route("editsystemparameter")]
        [AccessCodeAuthorize("SP03")]
        public async Task<IActionResult> EditSystemParameter([FromBody] EditSystemParameterRequest request) {
            EditDataResponse response;
            request.Refresh(HttpContext.User.Identity.Name, DateTime.Now);
            response = await _service.EditSystemParameterAsync(request);
            return Ok(response);
        }
        [HttpGet]
        [Route("deletesystemparameter")]
        [AccessCodeAuthorize("SP04")]
        public async Task<IActionResult> DeleteSystemParameter([FromQuery] DeleteSystemParameterRequest request) {
            EditDataResponse response;
            response = await _service.DeleteSystemParameterAsync(request);
            return Ok(response);
        }
    }
}
