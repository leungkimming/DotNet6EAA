namespace Client {
    public class ConfigUtil {
        private readonly IConfiguration _config;
        public bool IsEnableAAD {
            get {
                return Convert.ToBoolean(_config?.GetSection("AzureAd")?.GetValue(typeof(bool), "IsEnable")?.ToString()?.ToLower() ?? "false");
            }
        }
        public ConfigUtil(IConfiguration config) {
            _config = config;
        }
    }
}
