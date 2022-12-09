using Common;
using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class UnsupportedMediaTypeException : BaseCustomException<ValidationError> {
        public UnsupportedMediaTypeException() : base("") { }
        public UnsupportedMediaTypeException(string message) : base(message) { }
        protected UnsupportedMediaTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
