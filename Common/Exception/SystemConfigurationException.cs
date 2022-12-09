using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class SystemConfigurationException : BaseCustomException<SystemConfigurationError> {
        public SystemConfigurationException() { }
        public SystemConfigurationException(ErrorPayloadResponse<SystemConfigurationError> errorResponse) : base(errorResponse) { }
        protected SystemConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class SystemConfigurationErrorCategories {
        public static readonly SystemConfigurationError GridCommon2EndpointConfigError = new SystemConfigurationError {
            Code = "E1005",
        };
    }

    public class SystemConfigurationError : ValidationError {
        public SystemConfigurationError() { }
        public SystemConfigurationError(SystemConfigurationError inst) : base(inst) { }
    }
}
