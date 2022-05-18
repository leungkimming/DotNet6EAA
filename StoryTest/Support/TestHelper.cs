using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Reflection;
using TechTalk.SpecFlow.Assist;
using System.Collections;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;

namespace P6.StoryTest {
    [Binding]
    public static class TestHelper {
        public static ServiceProvider provider { get; set; }
        public static ScenarioContext context { get; set; }
        public static HttpClient client { get; set; }
        public static List<Claim> claims { get; set; }
        public static string LogonId { get; set; }
        public static void SetTable<T>(Table table, bool audit, string varName) where T : class {
            IEnumerable<T> listOfT = table.CreateSet<T>();
            if (audit) {
                foreach (T t in listOfT) {
                    t.GetType().GetMethod("Refresh").Invoke(t,
                        new object[] {LogonId, DateTime.Now });
                }
            }
            var db = provider.GetRequiredService<Data.EFContext>();
            DbSet<T> _dbSet = db.Set<T>();
            _dbSet.AddRange(listOfT);
            db.SaveChanges();
            context.Set(_dbSet.ToArray(), varName);
        }

        public static void SetClaims(Table table) {
            claims = new List<Claim>();
            foreach (var r in table.Rows) {
                claims.Add(new Claim(ClaimTypes.Role, r["Value"]));
            }
        }
        public static T SetDTO<T>(Table table, string varName) where T : class {
            T dto = table.CreateInstance<T>();
            context.Set<T>(dto, varName);
            return dto;
        }

        public async static Task PostAPI<T>(string vNameDTO, string apiRoute, string vNameResponse) where T : class {
            T request = context.Get<T>(vNameDTO);
            await RefreshToken();
            var response = await client.PostAsJsonAsync(apiRoute, request);
            context.Set(response, vNameResponse);
        }

        public static T SetDTOJson<T>(string json, string varName) where T : class {
            T dto = JsonConvert.DeserializeObject<T>(json);
            context.Set<T>(dto, varName);
            return dto;
        }
        public static bool Compare<T>(Table table, T dto) where T : class {
            bool match = true;
            string columns = "/";
            foreach (var r in table.Rows) {
                columns += r["Field"] + "/";
            }

            T tableDTO = table.CreateInstance<T>();
            var props = tableDTO.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props) {
                if (!columns.Contains("/" + prop.Name + "/")) {
                    continue;
                }
                var tableValue = prop.GetValue(tableDTO);
                var dtoValue = prop.GetValue(dto);
                if (prop.PropertyType == typeof(DateTime) ||
                    prop.PropertyType == typeof(Nullable<DateTime>)) {
                    if (((DateTime)tableValue).Date != ((DateTime)dtoValue).Date) {
                        match = false;
                        break;
                    }
                } else {
                    if (!tableValue.Equals(dtoValue)) {
                        match = false;
                        break;
                    }
                }
            }
            return match;
        }
        public async static Task<int> GetAPI<T>(string apiRoute, string vNameDTO) where T : class {
            await RefreshToken();
            var response = await client.GetAsync(apiRoute);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) {
                T users = System.Text.Json.JsonSerializer.Deserialize<T>(
                    content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                context.Set(users, vNameDTO);
            }
            return ((int)response.StatusCode);
        }

        public static bool CompareList<T, K>(Table table, T dto, string varName) where T : class where K : class {
            bool anymatch = false;

            foreach (K row in (IEnumerable)dto) {
                anymatch = Compare<K>(table, row);
                if (anymatch) {
                    context.Set<K>(row, varName);
                    break;
                }
            }
            return anymatch;
        }

        private async static Task RefreshToken()
        {
            var testResult = await client.GetAsync("Login");
            var cookies = testResult.Headers.GetValues("Set-Cookie").ToList();
            var token = cookies.Single(x => x.StartsWith("XSRF-TOKEN"))?.Substring($"{"XSRF-TOKEN"}=".Length).Split(";")[0];
            client.DefaultRequestHeaders.Remove("X-CSRF-TOKEN-HEADER");
            client.DefaultRequestHeaders.Add("X-CSRF-TOKEN-HEADER", token);
        }
    }
}
