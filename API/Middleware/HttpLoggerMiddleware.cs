using Service;
using System.Reflection;

namespace API {
    /// <summary>
    /// Middleware that is responsible for logging (into a json file) all the incoming requests
    /// and outgoing responses
    /// (TimeStamp, Route, Method, QueryParameters, Headers, Payload)
    /// </summary>
    internal class HttpLoggerMiddleware {
        private readonly RequestDelegate _next;
        private readonly RequestLogService _requestLog;
        private readonly List<string> _ignoreList = new List<string>(){ "/index.html", "/favicon.ico", "/Login","/reports/resources/js/telerikReportViewer"};

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpLoggerMiddleware(RequestDelegate next, RequestLogService requestLog) {
            _next = next;
            _requestLog = requestLog;
        }


        /// <summary>
        /// Main entry point of the Middleware
        /// Called by .NET request pipeline
        /// </summary>
        public async Task InvokeAsync(HttpContext context) {
            var request = context.Request;
            if (!string.IsNullOrEmpty(request.Path.Value) && !_ignoreList.Contains(request.Path.Value)) {
                var req = new Request(request);
                var resp = new Response(context.Response);
                await _next(context);
                string parameters = Convert.ToString(MessageBase.ToJson(MessageBase.ToDict(request.Query)));
                string requestString = Convert.ToString(await req.GetLogItemAsync());
                string respnseString = Convert.ToString(await resp.GetLogItemAsync());
                _requestLog.Log(DateTime.Now,
                    request.Path.Value,
                    request.Method,
                    parameters,
                    requestString,
                    respnseString
                );
            } else {
                await _next(context);
            }


        }
    }
}
