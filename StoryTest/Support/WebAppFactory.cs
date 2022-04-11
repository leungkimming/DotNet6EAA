using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace P6.StoryTest {
    public class WebAppFactory<T> : WebApplicationFactory<T> where T : class {
        IConfigurationRoot config;
        public string DefaultUserId { get; set; }
        public WebAppFactory(IConfigurationRoot config) : base() {
            this.config = config;
            DefaultUserId = "UserName";
        }
        protected override IHost CreateHost(IHostBuilder builder) {
            builder.ConfigureHostConfiguration(config => {
                config.AddInMemoryCollection(new Dictionary<string, string> {
                        { "ConnectionStrings:DDDConnectionString", this.config.GetConnectionString("DDDConnectionString") }
                    });
            });
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.ConfigureTestServices(services => {
                services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

                services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            });
            builder.UseEnvironment("SpecFlow");
        }
    }
}
