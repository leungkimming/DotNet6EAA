using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class NotFoundException : BaseCustomException<ValidationError> {
        public NotFoundException() { }
        public NotFoundException(ErrorPayloadResponse<ValidationError> errorResponse) : base(errorResponse) { }
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    public static class NotFoundErrorCategories {
        public static readonly ValidationError NotFoundError = new ValidationError {
            Code = "E0404",
        };
    }
}
