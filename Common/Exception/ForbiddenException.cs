using Common;
using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class ForbiddenException : BaseCustomException<ValidationError> {
        public ForbiddenException() : base("") { }
        public ForbiddenException(string message) : base(message) { }
        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
