using System.Net;
using System.Text.Json;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace API {
    public class ErrorHandler {
        private readonly RequestDelegate _next;
        public readonly ILogger<ErrorHandler> _logger;
        public string? RequestId { get; set; }
        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger) {
            _next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            int _statusCode = (int)HttpStatusCode.InternalServerError;

            try {
                await _next(context);
            } catch (Exception error) {
                //int bug = 0;
                //int bug1 = 2 / bug; // generate an exception in exception handler itself

                var response = context.Response;
                response.ContentType = "application/json";
                string _message = "";

                switch (error) {
                    case PayslipMonthAlreadyExistException ex:
                        _logger.LogWarning(100, ex.Message);
                        _statusCode = (int)HttpStatusCode.BadRequest;
                        _message = ex.Message;
                        break;
                    case UserAlreadyExistException ex:
                        _logger.LogWarning(101, ex.Message);
                        _statusCode = (int)HttpStatusCode.BadRequest;
                        _message = ex.Message;
                        break;
                    case RecordVersionException ex:
                        _logger.LogWarning(102, ex.Message);
                        _statusCode = (int)HttpStatusCode.BadRequest;
                        _message = ex.Message;
                        break;
                    default:
                        RequestId = Activity.Current?.Id ?? context.TraceIdentifier;
                        _logger.LogError(9998, error.Message + "\n" + error.StackTrace);
                        _statusCode = (int)HttpStatusCode.InternalServerError;
                        _message = String.Format(@"Service temporarily interrupted.Please report the problem to IT Help Desk with Trace Id ""{0}""",
                            RequestId);
                        break;
                }
                response.StatusCode = _statusCode;
                var result = JsonSerializer.Serialize(new { message = _message });
                await response.WriteAsync(result);
            }
        }
    }
}
