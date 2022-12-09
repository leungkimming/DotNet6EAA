
namespace Common {
    public interface IAppSettings {
        string SystemName { get; }
        string ConnectionString { get; }
        string ADDomain { get; }
        string Environment { get; }
        IAzureAdConfiguration AzureAd { get; }
        IGridCommon2Configuration GridCommon2 { get; }

    }
    public interface IAzureAdConfiguration {
        public string Instance { get; }
        public string TenantId { get; }
        public string WebClientId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }

    }


    public interface IReferenceServiceConfiguration {
        string Endpoint { get; }
    }

    public interface IGridCommon2Configuration : IReferenceServiceConfiguration {
        string Prefix { get; }
    }
}
