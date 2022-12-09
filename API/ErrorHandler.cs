using Common;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace API {
    public class ErrorHandler {
        private readonly IAppSettings _appSettings;
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;
        private readonly EventId _eventId;

        public ErrorHandler(RequestDelegate next, IAppSettings appSettings, ILogger<ErrorHandler> logger) {
            this._next = next;
            this._appSettings = appSettings;
            this._logger = logger;
            this._eventId = new EventId(9998);
        }

        public async Task Invoke(HttpContext context) {
            try {
                var requestLogMessage = await HttpObjectConverter.GetRequestLogMessage(context.Request);
                _logger.LogInformation(_eventId, requestLogMessage);
                await _next(context);
            } catch (Exception error) {
                string traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                // handle error and get status code
                var (handledError, code, unexpectedErrorMessage) = HandleError(error, traceId);

                // get error response payload
                var (contentWithStackTrace, contentWithoutStackTrace) = GetCustomizeErrorString(handledError, traceId, unexpectedErrorMessage);

                // logging
                _logger.LogError(_eventId, contentWithStackTrace);

                // return error response
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)code;
                if (this._appSettings.Environment != EnvrionmentConstant.PROD.Code) {
                    await response.WriteAsync(contentWithStackTrace);
                } else {
                    await response.WriteAsync(contentWithoutStackTrace);
                }
            }
        }

        private static (Exception handledError, HttpStatusCode code, string? message) HandleError(Exception error, string traceId) {
            // 400 Bad Request
            if (
                error is BadRequestException ||
               error is ConcurrencyUpdateException ||
               error is DBEntityUpdateException ||
               error is FilteringException) {
                return (error, HttpStatusCode.BadRequest, null);
            }

            // 403 Forbidden
            if (error is ForbiddenException || error is UserProfileForbiddenException) {
                return (error, HttpStatusCode.Forbidden, null);
            }

            // 404 NotFound
            if (
                error is NotFoundException ||
                error is Common.FileNotFoundException) {
                return (error, HttpStatusCode.NotFound, null);
            }

            // 409 Conflict
            if (
                error is RowVersionConflictException) {
                return (error, HttpStatusCode.Conflict, null);
            }

            // 415 Unsupported Media Type
            if (error is UnsupportedMediaTypeException) {
                return (error, HttpStatusCode.UnsupportedMediaType, null);
            }

            // 500 InternalServerError
            if (
                error is InternalException ||
                error is UserProfileInternalServerException ||
                error is UserProfileNotFoundInternalServerException ||
                error is SystemConfigurationException) {
                return (error, HttpStatusCode.InternalServerError, null);
            }

            // 501 NotImplemented
            if (error is NotImplementedException) {
                return (error, HttpStatusCode.NotImplemented, null);
            }

            // 409 Conflict for concurrency exception (RowVersion not match)
            if (error is DbUpdateConcurrencyException) {
                DbUpdateConcurrencyException? DbError = error as DbUpdateConcurrencyException;
                if (DbError != null) {
                    Exception handledError = error;

                    // extract entity name
                    string entityName = DbError.Entries[0].Entity.GetType().ToString();
                    entityName = entityName.Replace(Constants.DB_ENTITY_PREFIX, "");
                    var errorResponse = new ErrorPayloadResponse<ConcurrencyUpdateError>();
                    var newError = new ConcurrencyUpdateError(ConcurrencyUpdateErrorCategories.RowVersionConflictError);
                    newError.Extra = new Dictionary<string, object>() {
                        {"relatedEntity",entityName
}
                    };
                    errorResponse.Append(newError);
                    handledError.Data["ValidationErrorResponsePayload"] = errorResponse.Details;
                    return (handledError, HttpStatusCode.Conflict, null);
                }
            }

            // 400 BadRequest for exceed length exception
            if (error is DbUpdateException) {
                DbUpdateException? DbError = error as DbUpdateException;
                if (DbError != null && DbError.InnerException != null && DbError.InnerException.Message.Contains(Constants.DB_SAVE_LENGTH_ERROR)) {
                    Exception handledError = error;
                    var errorResponse = new ErrorPayloadResponse<ValidationError>();
                    ValidationError? newError = null;
                    newError = new ValidationError(DBEntityUpdateErrorCategories.LengthError);

                    // extract field name from error message
                    string errorMessage = DbError.InnerException.Message;
                    var indexOfStart = errorMessage.IndexOf(Constants.DB_SAVE_LENGTH_MESSAGE_FIELD_NAME_START_WORDING) + Constants.DB_SAVE_LENGTH_MESSAGE_FIELD_NAME_START_WORDING_LENGTH;
                    errorMessage = errorMessage.Substring(indexOfStart);
                    var indexOfEnd = errorMessage.IndexOf(Constants.DB_SAVE_LENGTH_MESSAGE_FIELD_NAME_END_WORDING);
                    string fieldName = errorMessage.Substring(0, indexOfEnd);
                    newError.Extra = new Dictionary<string, object>() {
                            {"relatedField", fieldName}
                        };

                    errorResponse.Append(newError);
                    handledError.Data["ValidationErrorResponsePayload"] = errorResponse.Details;
                    return (handledError, HttpStatusCode.BadRequest, null);
                }
            }

            // Unexpected error
            Exception unexpectedError = error;
            var unexpectedErrorResponse = new ErrorPayloadResponse<ValidationError>();
            unexpectedErrorResponse.Append(new ValidationError(UnexpectedErrorCategories.UnexpectedError));
            unexpectedError.Data["ValidationErrorResponsePayload"] = unexpectedErrorResponse.Details;
            string message = String.Format(@"Service temporarily interrupted.Please report the problem to IT Help Desk with Trace Id ""{0}""", traceId);
            return (unexpectedError, HttpStatusCode.InternalServerError, message);
        }

        private static (string contentWithStackTrace, string contentWithoutStackTrace) GetCustomizeErrorString(Exception error, string traceId, string? unexpectedErrorMessage = null) {
            var serializeOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            dynamic contentObjectWithStackTrace = new {
                traceId = traceId,
                details = error.Data["ValidationErrorResponsePayload"],
                message = unexpectedErrorMessage ?? error.Message,
                stackTrace = error.StackTrace?.Split("\n"),
                innerException = error.InnerException != null ? new {
                    message = error.InnerException.Message,
                    stackTrace = error.InnerException.StackTrace?.Split("\n")
                } : null
            };
            dynamic contentObjectWithoutStackTrace = new {
                traceId = traceId,
                details = error.Data["ValidationErrorResponsePayload"],
                message = unexpectedErrorMessage ?? error.Message,
            };
            string contentWithStackTrace = JsonSerializer.Serialize(contentObjectWithStackTrace, serializeOptions);
            string contentWithoutStackTrace = JsonSerializer.Serialize(contentObjectWithoutStackTrace, serializeOptions);
            return (contentWithStackTrace, contentWithoutStackTrace);
        }
    }
}


