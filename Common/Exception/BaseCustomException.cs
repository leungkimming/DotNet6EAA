using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class BaseCustomException<T> : Exception where T : ValidationError {
        public BaseCustomException() { }
        public BaseCustomException(string message) : base(message) { }
        public BaseCustomException(ErrorPayloadResponse<T> errorResponse) {
            this.Data["ValidationErrorResponsePayload"] = errorResponse.Details;
        }
        protected BaseCustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
