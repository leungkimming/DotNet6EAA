using Common;
using System.Runtime.Serialization;

namespace Data {
    [Serializable]
    public class InternalException : BaseCustomException<ValidationError> {
        public InternalException() : base("") { }
        public InternalException(string message) : base(message) { }
        protected InternalException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
