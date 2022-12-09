using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class ConcurrencyUpdateException : BaseCustomException<ConcurrencyUpdateError> {
        public ConcurrencyUpdateException() { }
        public ConcurrencyUpdateException(ErrorPayloadResponse<ConcurrencyUpdateError> errorResponse) : base(errorResponse) { }
        protected ConcurrencyUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class RowVersionConflictException : BaseCustomException<ConcurrencyUpdateError> {
        public RowVersionConflictException() { }
        public RowVersionConflictException(ErrorPayloadResponse<ConcurrencyUpdateError> errorResponse) : base(errorResponse) { }
        protected RowVersionConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DBEntityUpdateException : BaseCustomException<ValidationError> {
        public DBEntityUpdateException() { }
        public DBEntityUpdateException(ErrorPayloadResponse<ValidationError> errorResponse) : base(errorResponse) { }
        protected DBEntityUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class ConcurrencyUpdateErrorCategories {
        public static readonly ConcurrencyUpdateError RowVersionNotProvidedError = new ConcurrencyUpdateError
        {
            Code = "E1020",
        };

        public static readonly ConcurrencyUpdateError RowVersionConflictError = new ConcurrencyUpdateError
        {
            Code = "E1021",
        };
    };

    public static class DBEntityUpdateErrorCategories {
        public static readonly ValidationError LengthError = new ValidationError
        {
            Code = "E1027",
        };
    };
}
