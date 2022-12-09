using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class FilteringException : BaseCustomException<ValidationError> {
        public FilteringException() { }
        public FilteringException(ErrorPayloadResponse<ValidationError> errorResponse) : base(errorResponse) { }
        protected FilteringException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class FilteringErrorCategories {
        public static readonly ValidationError DateTimeValuesError = new ValidationError
        {
            Code = "E1028",
        };
    };
}
