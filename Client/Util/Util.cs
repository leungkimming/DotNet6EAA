using Common;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace Client {
    public class Util {
        private HttpClient Http { get; set; }
        private IJSRuntime _jSRuntime { get; set; }
        public Util(HttpClient _Http, IJSRuntime jsRuntime) {
            Http = _Http;
            _jSRuntime = jsRuntime;
        }
        public async Task RefreshToken() {
            AuthResult authResult;
            authResult = await Http.GetFromJsonAsync<AuthResult>("Login");

            Http.DefaultRequestHeaders.Remove("X-UserRoles");
            Http.DefaultRequestHeaders.Add("X-UserRoles", authResult.Token);

            var token = await _jSRuntime.InvokeAsync<string>("getCookie", "XSRF-TOKEN");
            Http.DefaultRequestHeaders.Remove("X-CSRF-TOKEN-HEADER");
            Http.DefaultRequestHeaders.Add("X-CSRF-TOKEN-HEADER", token);
        }
    }
}
