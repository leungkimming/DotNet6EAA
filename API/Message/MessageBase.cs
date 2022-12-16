using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace API {
    /// <summary>
    /// Abstract base class that represent an HTTP Message
    /// </summary>
    internal abstract class MessageBase {
        protected readonly object _message;
        public readonly IHeaderDictionary _headers;

        /// <summary>
        /// Constructor
        /// </summary>
        protected MessageBase(object message, IHeaderDictionary headers) {
            _message = message;
            _headers = headers;
        }

        /// <summary>
        /// Extracts the payload from the HTTP message;
        /// </summary>
        protected abstract Task<dynamic> GetPayload();

        /// <summary>
        /// Extracts the dictionary of the headers from the HTTP message
        /// </summary>
        private dynamic GetHeaders() {
            return ToJson(ToDict(_headers));
        }
        /// <summary>
        /// Converts a KeyValuePair collection to a proper dictionary
        /// </summary>
        public static IDictionary<TKey, TValue> ToDict<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> dict) {
            return dict?.ToDictionary(x => x.Key, x => x.Value);
        }
        /// <summary>
        /// Converts an object to a dynamic typed Json object
        /// </summary>
        public static dynamic ToJson(object obj) {
            if (obj is string) {
                return JsonConvert
                    .DeserializeObject<dynamic>(obj as string);
            }

            var str = JsonConvert
                .SerializeObject(obj, Formatting.Indented);

            return JsonConvert
                .DeserializeObject<dynamic>(str);
        }
        /// <summary>
        /// Gets the log item of the current message (headers & payload)
        /// </summary>
        internal async Task<dynamic> GetLogItemAsync() {
            return new {
                Headers = GetHeaders(),
                Body = await GetPayload(),
            };
        }
    }
}
