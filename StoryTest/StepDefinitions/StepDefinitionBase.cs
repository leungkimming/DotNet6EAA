using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace P6.StoryTest {
    public class StepDefinitionBase {
        public readonly ScenarioContext context;
        private readonly WebAppFactory<Program> webApplicationFactory;
        private string projectDir;
        private string configPath;
        public HttpClient client;
        public IConfigurationRoot config;
        public StepDefinitionBase(ScenarioContext context) {
            // Inject auto test connection string to application under test
            projectDir = Directory.GetCurrentDirectory();
            configPath = Path.Combine(projectDir, "appsettings.test.json");
            config = new ConfigurationBuilder().AddJsonFile(configPath).Build();

            this.context = context;
            this.webApplicationFactory = new WebAppFactory<Program>(config);
            this.webApplicationFactory.DefaultUserId = "tester";

            // create an http client of the application under test
            client = this.webApplicationFactory.CreateClient();
            //client.BaseAddress = (new Uri("https://localhost:44355"));

        }

        public void SetAuthorization(string auth) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth);
        }

        public void SetLogonId(string LogonId) {
            client.DefaultRequestHeaders.Remove(TestAuthHandler.UserId);
            client.DefaultRequestHeaders.Add(TestAuthHandler.UserId, LogonId);
        }
    }
}
