using Common.DTOs;
using System.Net.Http.Json;

namespace Client.Util
{
    public class Util
    {
        private HttpClient Http { get; set; }
        public Util(HttpClient _Http)
        {
            Http = _Http;
        }
        public async Task RefreshToken()
        {
            AuthResult authResult;
            authResult = await Http.GetFromJsonAsync<AuthResult>("Login");

            Http.DefaultRequestHeaders.Remove("X-UserRoles");
            Http.DefaultRequestHeaders.Add("X-UserRoles", authResult.Token);
        }
    }
}
