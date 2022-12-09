using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class FileValidationException : BaseCustomException<FileValidationError> {
        public FileValidationException() { }
        public FileValidationException(ErrorPayloadResponse<FileValidationError> errorResponse) : base(errorResponse) { }
        protected FileValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class FileNotFoundException : BaseCustomException<FileValidationError> {
        public FileNotFoundException() { }
        public FileNotFoundException(ErrorPayloadResponse<FileValidationError> errorResponse) : base(errorResponse) { }
        protected FileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class FileErrorCategories {
        public static readonly FileValidationError FileNotFoundInDBError = new FileValidationError {
            Code = "E0910",
        };

        public static readonly FileValidationError FileNotFoundInSPOError = new FileValidationError {
            Code = "E0911",
        };

        public static readonly FileValidationError FileUrlNotFoundError = new FileValidationError {
            Code = "E0912",
        };
    };
    public class FileValidationError : ValidationError {
        public FileValidationError() { }
        public FileValidationError(FileValidationError inst) : base(inst) { }
    }
}
