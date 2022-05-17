﻿using Common;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Client {
    public class HttpUtil {
        private readonly HttpClient _http;
        private IJSRuntime _jSRuntime;
        private IAccessTokenProvider _tokenProvider;
        public string AccessToken { get; set; }
        public HttpUtil(HttpClient http, IJSRuntime jsRuntime, IAccessTokenProvider tokenProvider) {
            _http = http;
            _jSRuntime = jsRuntime;
            _tokenProvider = tokenProvider;
        }
        public async Task<HttpResponseMessage> PostAsync(string? requestUri, HttpContent? content) {
            await RefreshToken();
            return await _http.PostAsync($"{requestUri}", content);
        }
        public async Task<HttpResponseMessage> GetAsync(string? requestUri) {
            await RefreshToken();
            return await _http.GetAsync($"{requestUri}");
        }
        public async Task<HttpResponseMessage> PostAsJsonAsync(string? requestUri, object? content) {
            await RefreshToken();
            return await _http.PostAsJsonAsync($"{requestUri}", content);
        }
        public async Task<T> GetFromJsonAsync<T>(string? requestUri) {
            await RefreshToken();
            return await _http.GetFromJsonAsync<T>($"{requestUri}");
        }
        public async Task SetAccessToken() {
            var accessTokenResult = await _tokenProvider.RequestAccessToken();
            AccessToken = string.Empty;

            if (accessTokenResult.TryGetToken(out var token)) {
                AccessToken = $"Bearer {token.Value}";
            }
        }
        public async Task RefreshToken() {
            AuthResult authResult;
            authResult = await _http.GetFromJsonAsync<AuthResult>("Login") ?? new AuthResult();

            _http.DefaultRequestHeaders.Remove("X-UserRoles");
            _http.DefaultRequestHeaders.Add("X-UserRoles", authResult.Token);

            var token = await _jSRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");
            _http.DefaultRequestHeaders.Remove("X-CSRF-TOKEN-HEADER");
            _http.DefaultRequestHeaders.Add("X-CSRF-TOKEN-HEADER", token);
        }
        public async Task RefreshToken(Dictionary<string, object> requestHeaders) {
            AuthResult authResult;
            authResult = await _http.GetFromJsonAsync<AuthResult>("Login") ?? new AuthResult();
            requestHeaders.Add("X-UserRoles", authResult.Token);
            var token = await _jSRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");
            requestHeaders.Add("X-CSRF-TOKEN-HEADER", token);
            await SetAccessToken();
            requestHeaders.Add("authorization", AccessToken);
        }
    }
}
