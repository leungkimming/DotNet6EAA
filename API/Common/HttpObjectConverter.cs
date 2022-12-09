using System.Net;
using System.Text;

namespace API {
    public static class HttpObjectConverter {
        private const int REQUEST_SIZE = 1000;

        public async static Task<string> GetRequestLogMessage(HttpRequest httpRequest) {
            if (httpRequest == null) {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            var stringBuilder = new StringBuilder(REQUEST_SIZE);
            stringBuilder.AppendLine($"--- REQUEST {httpRequest.HttpContext.TraceIdentifier}: BEGIN ---");
            stringBuilder.AppendLine($"{httpRequest.Method} {httpRequest.Path}{httpRequest.QueryString.ToUriComponent()} {httpRequest.Protocol}");

            if (httpRequest.Headers.Any()) {
                foreach (var header in httpRequest.Headers) {
                    stringBuilder.AppendLine($"{header.Key}: {header.Value}");
                }
            }

            stringBuilder.AppendLine();

            httpRequest.EnableBuffering();
            StreamReader reader = new StreamReader(httpRequest.Body);
            string body = await reader.ReadToEndAsync();
            httpRequest.Body.Seek(0, SeekOrigin.Begin);
            stringBuilder.AppendLine(body);

            stringBuilder.AppendLine($"--- REQUEST {httpRequest.HttpContext.TraceIdentifier}: END ---");

            var result = stringBuilder.ToString();
            return result;
        }
    }
}
