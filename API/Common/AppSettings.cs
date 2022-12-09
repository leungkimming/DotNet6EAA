using Common;

namespace API {
    public class AppSettings : IAppSettings {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration) {
            this._configuration = configuration;
            this.Swagger = new SwaggerConfiguration(_configuration.GetSection("Swagger"));
            this.GridCommon2 = new GridCommon2Configuration(_configuration.GetSection("GridCommon2"));
            this.AzureAd = new AzureAdConfiguration(_configuration.GetSection("AzureAd"));
        }

        public string SystemName => _configuration.GetValue<string>("SystemName");
        public string Environment => _configuration.GetValue<string>("AppEnvironment");
        public string ADDomain=> _configuration.GetValue<string>("ADDomain");
        public string ConnectionString => _configuration.GetConnectionString("DDDConnectionString");

        public SwaggerConfiguration Swagger { get; private set; }
        public IGridCommon2Configuration GridCommon2 { get; private set; }
        public IAzureAdConfiguration AzureAd { get; private set; }
    }
    public class SwaggerConfiguration {
        private readonly IConfiguration _configuration;
        public SwaggerConfiguration(IConfiguration configuration) {
            this._configuration = configuration;
        }
        public string Version => _configuration.GetValue<string>("Version");
    }
    public class ReferenceServiceConfiguration : IReferenceServiceConfiguration {
        protected IConfiguration _configuration;
        public ReferenceServiceConfiguration(IConfiguration configuration) {
            this._configuration = configuration;
        }

        public string Endpoint => _configuration.GetValue<string>("Endpoint");
    }

    public class GridCommon2Configuration : ReferenceServiceConfiguration, IGridCommon2Configuration {
        public GridCommon2Configuration(IConfiguration configuration) : base(configuration) { }

        public string Prefix => _configuration.GetValue<string>("Prefix");
    }

    public class AzureAdConfiguration : IAzureAdConfiguration {
        private readonly IConfiguration _configuration;
        public AzureAdConfiguration(IConfiguration configuration) {
            this._configuration = configuration;
        }
        public string Instance => _configuration.GetValue<string>("Instance");
        public string TenantId => _configuration.GetValue<string>("TenantId");
        public string WebClientId => _configuration.GetValue<string>("WebClientId");
        public string ClientId => _configuration.GetValue<string>("ClientId");
        public string ClientSecret => _configuration.GetValue<string>("ClientSecret");
    }


}
