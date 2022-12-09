using System.Runtime.Serialization;
namespace Common {
    [Serializable]
    public class UserProfileNotFoundInternalServerException : BaseCustomException<GetUserProfileError> {
        public UserProfileNotFoundInternalServerException() { }
        public UserProfileNotFoundInternalServerException(ErrorPayloadResponse<GetUserProfileError> errorResponse) : base(errorResponse) { }
        protected UserProfileNotFoundInternalServerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UserProfileInternalServerException : BaseCustomException<GetUserProfileError> {
        public UserProfileInternalServerException() { }
        public UserProfileInternalServerException(ErrorPayloadResponse<GetUserProfileError> errorResponse) : base(errorResponse) { }
        protected UserProfileInternalServerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class UserProfileForbiddenException : BaseCustomException<GetUserProfileError> {
        public UserProfileForbiddenException() { }
        public UserProfileForbiddenException(ErrorPayloadResponse<GetUserProfileError> errorResponse) : base(errorResponse) { }
        protected UserProfileForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public static class GetUserProfileErrorCategories {

        public static readonly GetUserProfileError NotFoundError = new GetUserProfileError {
            Code = "E1010",
        };

        public static readonly GetUserProfileError ForbiddenError = new GetUserProfileError {
            Code = "E1011",
        };

        public static readonly GetUserProfileError VendorNoNotFoundError = new GetUserProfileError {
            Code = "E1012",
        };

        public static readonly GetUserProfileError BusinessUnitNotFoundError = new GetUserProfileError {
            Code = "E1015",
        };

        public static readonly GetUserProfileError UserNameNotFoundError = new GetUserProfileError {
            Code = "E1016",
        };

        public static readonly GetUserProfileError SamAccountNameNotFoundError = new GetUserProfileError {
            Code = "E1006",
        };

        public static readonly GetUserProfileError GC2GetUserProfileError = new GetUserProfileError {
            Code = "E1007",
        };

        public static readonly GetUserProfileError GC2ResponseNotFoundError = new GetUserProfileError {
            Code = "E1008",
        };

        public static readonly GetUserProfileError GC2UserProfileMappingError = new GetUserProfileError {
            Code = "E1009",
        };
    };
    public class GetUserProfileError : ValidationError {
        public GetUserProfileError() { }
        public GetUserProfileError(GetUserProfileError inst) : base(inst) { }
    }
}
