using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class JWTException : BaseCustomException<ValidationError> {
        public JWTException() : base("") { }
        public JWTException(string message) : base(message) { }
        protected JWTException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
