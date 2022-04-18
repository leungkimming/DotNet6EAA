﻿using Common;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API {
    [ApiController]
    [Route("systemparameters")]
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
            response = await _service.AddNewAsync(request);
            return Ok(response);
        }
    }
}