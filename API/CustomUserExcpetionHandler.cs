using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using Common.Shared;

namespace API
{
    public  class CustomUserExceptionHandler
    {
        private  ILogger<CustomUserExceptionHandler> _logger { get; set; }
        public CustomUserExceptionHandler(ILogger<CustomUserExceptionHandler> logger)
        {
            _logger = logger;
        }
        public Task HandleExceptionResponse(HttpContext httpContext, Func<Task> next)
        {
            // Exception handler middleware has everything already set up for us
            var exceptionDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
            var exceptionError = exceptionDetails?.Error;
            var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
            var error = exceptionHandlerPathFeature.Error;

            // add custom logic here

            var response = httpContext.Response;
            response.ContentType = "application/json";
            int _statusCode = (int)HttpStatusCode.InternalServerError;
            string _message = "";

            switch (error)
            {
                //case PayslipMonthAlreadyExistException ex:
                //    _logger.LogError(100, ex.Message);
                //    _statusCode = (int)HttpStatusCode.BadRequest;
                //    _message = ex.Message;
                //    break;
                //case UserAlreadyExistException ex:
                //    _logger.LogWarning(3000, ex.Message);
                //    _statusCode = (int)HttpStatusCode.BadRequest;
                //    _message = ex.Message;
                //    break;
                default:
                    _logger.LogError(9999, error.Message + "\n" + error.StackTrace);
                    _statusCode = (int)HttpStatusCode.InternalServerError;
                    _message = "Error:" + _statusCode + ", Service temporarily interrupted. Please retry. If the error persists, please call IT Help Desk";
                    break;
            }

            response.StatusCode = _statusCode;
            var result = JsonSerializer.Serialize(new { message = _message });
            response.WriteAsync(result);

            return Task.CompletedTask;
        }
    }
}
