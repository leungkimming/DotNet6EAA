namespace Client {
    public class HttpUtil {
        private readonly Util _util;
        private readonly HttpClient _http;
        public HttpUtil(Util util, HttpClient http) {
            _util = util;
            _http = http;
        }
        public async Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content) {
            await _util.RefreshToken();
            return await _http.PostAsync($"{requestUri}", content);           
        }
        public async Task<HttpResponseMessage> GetAsync(string? requestUri) {
            return await _http.GetAsync($"{requestUri}");
        }
    }
}
