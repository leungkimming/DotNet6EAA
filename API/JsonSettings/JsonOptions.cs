using System.Text.Json;

namespace API {
    public static class JsonOptions {

        private static JsonSerializerOptions _options;

        public static JsonSerializerOptions SerializerOptions {
            get {
                if (_options is null) {
                    _options = new JsonSerializerOptions();
                    _options.PropertyNameCaseInsensitive = false;
                    _options.PropertyNamingPolicy = null;
                    _options.Converters.AddDTOConverters();
                }
                return _options;
            }
            set {
                if (_options is null) {
                    _options = value;
                }
            }
        }
    }
}
