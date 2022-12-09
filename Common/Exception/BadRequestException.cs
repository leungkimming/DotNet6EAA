using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class BadRequestException : BaseCustomException<ValidationError> {
        public BadRequestException() : base() { }
        public BadRequestException(ErrorPayloadResponse<ValidationError> errorResponse) : base(errorResponse) { }
        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class BadRequestErrorCategories {
        public static readonly ValidationError TaintedParameterWhiteListCheckingError = new ValidationError {
            Code = "E1026"
        };
    }
}
