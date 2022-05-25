using Common;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net;
using Microsoft.AspNetCore.Components;

namespace Client {
    public class HttpUtil {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jSRuntime;
        private readonly NavigationManager _navigationManager;
        public HttpUtil(HttpClient http, IJSRuntime jsRuntime, IServiceProvider serviceProvider, NavigationManager navigationManager) {
            _http = http;
            _jSRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }
        public async Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content) {
            await RefreshToken();
            var response=await _http.PostAsync($"{requestUri}", content);
            ErrorHandler(response.StatusCode);
            return response;
        }
        public async Task<HttpResponseMessage> GetAsync(string? requestUri) {
            await RefreshToken();
            var response=await _http.GetAsync($"{requestUri}");
            ErrorHandler(response.StatusCode);
            return response;
        }

        private void ErrorHandler(HttpStatusCode? httpStatusCode) {
            if (httpStatusCode == HttpStatusCode.Forbidden) {
                _navigationManager.NavigateTo($"Error/{ErrorConstant.AuthorizationError.Code}");
                throw new OperationCanceledException($"{HttpStatusCode.Forbidden}:you don't has permission.");
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync(string? requestUri, object? content) {
            await RefreshToken();
            var response=await _http.PostAsJsonAsync($"{requestUri}", content);
            ErrorHandler(response.StatusCode);
            return response;
        }
        public async Task<T> GetFromJsonAsync<T>(string? requestUri) {
            await RefreshToken();
            try {
                var response=await _http.GetFromJsonAsync<T>($"{requestUri}");
                return response;
            } catch (HttpRequestException httpEx){
                ErrorHandler(httpEx.StatusCode);
            }
            return default(T);
        }
        public async Task RefreshToken() {
            var refreshToken = await _http.GetFromJsonAsync<RefreshTokenResponse>("Login?force=false") ?? null;

            var token = await _jSRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");
            _http.DefaultRequestHeaders.Remove("X-CSRF-TOKEN-HEADER");
            _http.DefaultRequestHeaders.Add("X-CSRF-TOKEN-HEADER", token);
        }
        public async Task RefreshToken(Dictionary<string, object> requestHeaders) {
            var refreshToken = await _http.GetFromJsonAsync<RefreshTokenResponse>("Login?force=false") ?? null;

            var token = await _jSRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");
            requestHeaders.Add("X-CSRF-TOKEN-HEADER", token);
        }
    }
}
