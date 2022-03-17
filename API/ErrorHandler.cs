using System.Net;
using System.Text.Json;
using Common.Shared;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        public readonly ILogger<ErrorHandler> _logger;
        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            int _statusCode = (int)HttpStatusCode.InternalServerError;

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string _message = "";

                switch (error)
                {
                    case PayslipMonthAlreadyExistException ex:
                        _logger.LogError(100, ex.Message);
                        _statusCode = (int)HttpStatusCode.BadRequest;
                        _message = ex.Message;
                        break;
                    case UserAlreadyExistException ex:
                        _logger.LogWarning(3000, ex.Message);
                        _statusCode = (int)HttpStatusCode.BadRequest;
                        _message = ex.Message;
                        break;
                    default:
                        _logger.LogError(9999, error.Message + "\n" + error.StackTrace);
                        _statusCode = (int)HttpStatusCode.InternalServerError;
                        _message = "Error:" + _statusCode + ", Service temporarily interrupted. Please retry. If the error persists, please call IT Help Desk";
                        break;
                }
                response.StatusCode = _statusCode;
                var result = JsonSerializer.Serialize(new { message = _message });
                await response.WriteAsync(result);
            }
        }
    }
}
